using Microsoft.AspNetCore.Identity;
using MindHeal.FileManagers;
using MindHeal.Implementations.Repositories;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;
using System.Text.RegularExpressions;

namespace MindHeal.Implementations.Services
{
    public class TherapistService : ITherapistService
    {
        private readonly ITherapistRepository _therapistRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileManager _fileManager;
        private readonly INotificationMessage _notificationMessage;
        private readonly UserManager<User> _userManager;
        private readonly IIssuesRepository _issuesRepository;
        private readonly IRoleRepository _roleRepository;
        public TherapistService(ITherapistRepository therapistRepository, IUserRepository userRepository, IFileManager fileManager, UserManager<User> userManager, INotificationMessage notificationMessage, IIssuesRepository issuesRepository, IRoleRepository roleRepository)
        {
            _therapistRepository = therapistRepository;
            _userRepository = userRepository;
            _fileManager = fileManager;
            _userManager = userManager;
            _notificationMessage = notificationMessage;
            _issuesRepository = issuesRepository;
            _roleRepository = roleRepository;
        }
        private static bool ValidatePassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[A-Z])(?=.*[@#$%^&+=])(?=.{8,})");
            return regex.IsMatch(password);
        }
        public async Task<BaseResponse<TherapistDto>> Create(CreateTherapistRequestModel model)
        {
            bool isValid = ValidatePassword(model.Password);
            if (!isValid)
            {
                return new BaseResponse<TherapistDto>
                {
                    Message = "Password is invalid. Password must be at least 8 characters long, contain at least one capital letter, and a special character.",
                    Status = false,

                };
            }


            var checkIfExist = await _therapistRepository.CheckIfExist(model.Email);
            if (checkIfExist != null) return new BaseResponse<TherapistDto>
            {
                Message = "User already exist",
                Status = false,
            };
            var role = await _roleRepository.Get<Role>(b => b.Name == "Therapist");

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                IsDeleted = false,


            };
            await _userManager.CreateAsync(user, model.Password);
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
                Role = role,
                User = user,
            };

            await _roleRepository.Add<UserRole>(userRole);
            var certificatefile = await _fileManager.UploadFileToSystem(model.Certificate);
            var credentialsfile = await _fileManager.UploadFileToSystem(model.Credential);
            var profilepicturefile = await _fileManager.UploadFileToSystem(model.ProfilePicture);
            var issues = new List<TherapistIssues>();

            var therapist = new Therapist
            {
                Certificate = certificatefile.Data.Name,
                Credential = credentialsfile.Data.Name,
                ProfilePicture = profilepicturefile.Data.Name,
                UserName = model.UserName,
                RegNo = model.RegNo,
                Description = model.Description,
                UserId = user.Id,
                User = user
            };
            await _therapistRepository.Add(therapist);
            foreach (var item in model.IssueIds)
            {
                var issue = await _issuesRepository.Get<Issues>(item);
                var therapistIssue = new TherapistIssues
                {
                    Issues = issue,
                    IssuesId = issue.Id,
                    TherapistId = therapist.Id,
                    Therapist = therapist,
                };
                await _therapistRepository.Add<TherapistIssues>(therapistIssue);
            }

            user.Therapist = therapist;
            var request = new WhatsappMessageSenderRequestModel { ReciprantNumber = model.PhoneNumber, MessageBody = "Therapist created Successfully" };
            await _notificationMessage.SendWhatsappMessageAsync(request);

            return new BaseResponse<TherapistDto>
            {
                Message = "Therapist created successfully",
                Status = true,
                Data = new TherapistDto
                {
                    FirstName = therapist.User.FirstName,
                    LastName = therapist.User.LastName,
                    Email = therapist.User.Email,
                    Gender = therapist.User.Gender,
                }
            };


        }

        public async Task<BaseResponse<TherapistDto>> Delete(Guid id)
        {
            var therapist = await _therapistRepository.Get<Therapist>(id);
            if (therapist == null) return new BaseResponse<TherapistDto>
            {
                Message = "Therapist Not Found",
                Status = false,
            };

            therapist.IsDeleted = true;
            await _therapistRepository.save();
             
            return new BaseResponse<TherapistDto>
            {
                Message = "Deleted Successfully",
                Status = true,
            };
        }

        public async Task<IEnumerable<TherapistDto>> GetAll()
        {
            var therapists = await _therapistRepository.GetAllTherapist();
            var listOftherapists = therapists.Select(a => new TherapistDto
            {
                Id = a.Id,
                UserId = a.UserId,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                Email = a.User.Email,
                RegNo = a.RegNo,
                Certificate = a.Certificate,
                IsAvailable = a.IsAvalaible,
                Credential = a.Credential,
                ProfilePicture = a.ProfilePicture,
                PhoneNumber = a.User.PhoneNumber,
                Description = a.Description,
                TherapistIssues = a.TherapistIssues,
            }).ToList();
            return listOftherapists;
        }

        public async Task<IEnumerable<TherapistDto>> GetAllAvailableTherapist()
        {
            var therapists = await _therapistRepository.GetAllAvailableTherapist();
            var listOftherapists = therapists.Select(a => new TherapistDto
            {
                Id = a.Id,
                //UserId = a.UserId,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                Email = a.User.Email,
                RegNo = a.RegNo,
            }).ToList();
            return listOftherapists;
        }

        public async Task<List<UserDto>> GetAllTherapistByChat()
        {
            var therapists = await _therapistRepository.GetAllTherapist();
            var listOfTherapists = therapists.Select(a => new UserDto
            {
                Id = a.UserId,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
            }).ToList();
            return listOfTherapists;
        }

        public async Task<BaseResponse<TherapistDto>> GetTherapist(Guid id)
        {
            var therapist = await _therapistRepository.GetTherapist(id);
            if (therapist == null) return new BaseResponse<TherapistDto>
            {
                Message = "Therapist not found",
                Status = false,
            };

            return new BaseResponse<TherapistDto>
            {
                Message = "Successful",
                Status = true,
                Data = new TherapistDto
                {
                    Id = therapist.Id,
                    FirstName = therapist.User.FirstName,
                    LastName = therapist.User.LastName,
                    PhoneNumber = therapist.User.PhoneNumber,
                    Email = therapist.User.Email,
                    RegNo = therapist.RegNo,
                    Certificate = therapist.Certificate,
                    Credential = therapist.Credential,
                    ProfilePicture = therapist.ProfilePicture,
                    Gender = therapist.User.Gender,

                }
            };
        }

        public Task<BaseResponse<TherapistDto>> Update(Guid id, UpdateTherapistRequestModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<TherapistDto>>> ViewapprovedTherapist()
        {
            var therapist = await _therapistRepository.GetApprovedTherapist();
            if (therapist == null) return new BaseResponse<IEnumerable<TherapistDto>>
            {
                Message = "Therapist Not Found",
                Status = false,
            };
            var listoftherapists = therapist.Select(a => new TherapistDto
            {
                Id = a.Id,
                UserId = a.UserId,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                PhoneNumber = a.User.PhoneNumber,
                RegNo = a.RegNo,
                Certificate = a.Certificate,
                Credential = a.Credential,
            }).ToList();
            return new BaseResponse<IEnumerable<TherapistDto>>
            {
                Message = "Successful",
                Status = true,
                Data = listoftherapists
            };
        }

        public async Task<BaseResponse<IEnumerable<TherapistDto>>> ViewRejectedTherapist()
        {
            var therapists = await _therapistRepository.GetRejectedTherapist();
            if (therapists == null) return new BaseResponse<IEnumerable<TherapistDto>>
            {
                Message = "Therapist Not Found",
                Status = false,
            };
            var listOftherapists = therapists.Select(a => new TherapistDto
            {
                Id = a.Id,
                UserId = a.UserId,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                PhoneNumber = a.User.PhoneNumber,
                RegNo = a.RegNo,
                Certificate = a.Certificate,
                Credential = a.Credential,
            }).ToList();

            return new BaseResponse<IEnumerable<TherapistDto>>
            {
                Message = "Successful",
                Status = true,
                Data = listOftherapists
            };
        }

        public async Task<BaseResponse<IEnumerable<TherapistDto>>> ViewUnapprovedTherapist()
        {
            var therapist = await _therapistRepository.GetAllUnApprovedTherapist();
            if (therapist == null) return new BaseResponse<IEnumerable<TherapistDto>>
            {
                Message = "Therapist Not Found",
                Status = false,
            };
            var listOftherapists = therapist.Select(a => new TherapistDto
            {
                Id = a.Id,
                UserId = a.UserId,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                PhoneNumber = a.User.PhoneNumber,
                RegNo = a.RegNo,
                Certificate = a.Certificate,
                Credential = a.Credential,
            }).ToList();
            return new BaseResponse<IEnumerable<TherapistDto>>
            {
                Message = "Successful",
                Status = true,
                Data = listOftherapists
            };
        }
    }
}
