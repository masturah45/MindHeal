using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MindHeal.FileManagers;
using MindHeal.Implementations.Repositories;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;
using MindHeal.Models.Entities.Enum;
using System.Text.RegularExpressions;

namespace MindHeal.Implementations.Services
{
    public class TherapistService : ITherapistService
    {
        private readonly ITherapistRepository _therapistRepository;
        private readonly IFileManager _fileManager;
        private readonly INotificationMessage _notificationMessage;
        private readonly UserManager<User> _userManager;
        private readonly IIssuesRepository _issuesRepository;
        public TherapistService(ITherapistRepository therapistRepository,  IFileManager fileManager, UserManager<User> userManager, INotificationMessage notificationMessage, IIssuesRepository issuesRepository)
        {
            _therapistRepository = therapistRepository;
            _fileManager = fileManager;
            _userManager = userManager;
            _notificationMessage = notificationMessage;
            _issuesRepository = issuesRepository;
        }
        private static bool ValidatePassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[A-Z])(?=.*[@#$%^&+=])(?=.{8,})");
            return regex.IsMatch(password);
        }

        public async Task<BaseResponse<TherapistDto>> Approve(Guid id)
        {
            var therapist = await _therapistRepository.GetTherapist(id);
            if (therapist == null) return new BaseResponse<TherapistDto>
            {
                Message = "Not Successful",
                Status = false,
            };
            therapist.Status = Approval.Approved;
            therapist.IsDeleted = false;
            var user = await _userManager.FindByIdAsync(therapist.UserId);
            user.EmailConfirmed = true;
            await _therapistRepository.save();
            return new BaseResponse<TherapistDto>
            {
                Message = "Delete Successful",
                Status = true,
                Data = new TherapistDto
                {
                    Id = therapist.Id,
                    FirstName = therapist.User.FirstName,
                    LastName = therapist.User.LastName,
                    PhoneNumber = therapist.User.PhoneNumber,
                    RegNo = therapist.RegNo,
                    Certificate = therapist.Certificate,
                    Credential = therapist.Credential,
                }

            };
        }

        public async Task<BaseResponse<TherapistDto>> RejectapprovedTherapist(Guid id)
        {
            var therapist = await _therapistRepository.GetTherapist(id);
            if (therapist == null) return new BaseResponse<TherapistDto>
            {
                Message = "Not Successful",
                Status = false,
            };
            therapist.Status = Approval.Rejected;
            await _therapistRepository.save();

            return new BaseResponse<TherapistDto>
            {
                Message = "Delete Successful",
                Status = true,
                Data = new TherapistDto
                {
                    Id = therapist.Id,
                    FirstName = therapist.User.FirstName,
                    LastName = therapist.User.LastName,
                    PhoneNumber = therapist.User.PhoneNumber,
                    RegNo = therapist.RegNo,
                    Certificate = therapist.Certificate,
                    Credential = therapist.Credential,
                }
            };

        }

        public async Task<BaseResponse<TherapistDto>> Create(CreateTherapistRequestModel model)
        {
            try
            {

            var checkIfExist = await _userManager.FindByEmailAsync(model.Email);
            if (checkIfExist != null)
                return new BaseResponse<TherapistDto>
                {
                    Message = "User already exist",
                    Status = false,
                };

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
                UserName = model.Email,
            };
             var createUser = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Therapist");

            var certificatefile = await _fileManager.UploadFileToSystem(model.Certificate);
            var credentialsfile = await _fileManager.UploadFileToSystem(model.Credential);
            var profilepicturefile = await _fileManager.UploadFileToSystem(model.ProfilePicture);
            var issues = new List<TherapistIssues>();

            var therapist = new Therapist
            {
                User = user,
                UserId = user.Id,
                Certificate = certificatefile.Data.Name,
                Credential = credentialsfile.Data.Name,
                ProfilePicture = profilepicturefile.Data.Name,
                UserName = model.UserName,
                RegNo = model.RegNo,
                Description = model.Description,
            };
            await _therapistRepository.Add(therapist);
            await _therapistRepository.save();
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

            //user.Therapist = therapist;
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
            catch (Exception ex)
            {
                // Handle the exception here, you can log it, return an error response, etc.
                // For example, you can log the exception and return a generic error response
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new BaseResponse<TherapistDto>
                {
                    Message = "An error occurred while creating the client",
                    Status = false,
                };
            }

        }

        public async Task<BaseResponse<TherapistDto>> Delete(Guid id)
        {
            //var therapist = await _therapistRepository.GetTherapist(id);
            var therapist = await _therapistRepository.GetTherapistByUserId(id.ToString());
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
           if(therapists.Count() > 0)
            {
                var listOftherapists = therapists.Select(a => new TherapistDto
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    FirstName = a.User.FirstName,
                    LastName = a.User.LastName,
                    ProfilePicture = a.ProfilePicture,
                    PhoneNumber = a.User.PhoneNumber,
                    Description = a.Description,
                    TherapistIssues = a.TherapistIssues,
                }).ToList();
                return listOftherapists;
            }
            return null;
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
            var user = await _therapistRepository.GetTherapistByUserId(therapist.UserId);

            if (user == null) return new BaseResponse<TherapistDto>
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
                    Id = user.Id,
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName,
                    PhoneNumber = user.User.PhoneNumber,
                    Email = user.User.Email,
                    RegNo = user.RegNo,
                    Gender = user.User.Gender,
                    UserId = user.UserId,
                }
            };
        }
        public async Task<BaseResponse<TherapistDto>> GetTherapistForProfile(string id)
        {
            //var therapist = await _therapistRepository.GetTherapist(id);
            var user = await _therapistRepository.GetTherapistByUserId(id);

            if (user == null) return new BaseResponse<TherapistDto>
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
                    Id = user.Id,
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName,
                    PhoneNumber = user.User.PhoneNumber,
                    Email = user.User.Email,
                    RegNo = user.RegNo,
                    Gender = user.User.Gender,
                    UserId = user.UserId,
                    IsAvailable = user.IsAvalaible,
                }
            };
        }
        public async Task<BaseResponse<TherapistDto>> Update(Guid id, UpdateTherapistRequestModel model)
        {
            var request = new WhatsappMessageSenderRequestModel { ReciprantNumber = model.PhoneNumber, MessageBody = "Therapist updated successfully" };
            await _notificationMessage.SendWhatsappMessageAsync(request);
            //var therapist = await _therapistRepository.GetTherapist(id);
            var therapist = await _therapistRepository.GetTherapistByUserId(id.ToString());
            if (therapist == null) return new BaseResponse<TherapistDto>
            {
                Message = "therapist not found",
                Status = false,
            };

            therapist.User.FirstName = model.FirstName;
            therapist.User.LastName = model.LastName;
            therapist.User.Email = model.Email;
            therapist.User.PhoneNumber = model.PhoneNumber;
            therapist.Description = model.Description;
            therapist.RegNo = model.RegNo;
            therapist.DateCreated = DateTime.Now;
            therapist.DateUpdated = DateTime.Now;
            therapist.IsDeleted = false;

            await _therapistRepository.Update(therapist);

            return new BaseResponse<TherapistDto>
            {
                Message = "Successful",
                Status = true,
                Data = new TherapistDto
                {
                    FirstName = therapist.User.FirstName,
                    LastName = therapist.User.LastName,
                }
            };
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

        public async Task<BaseResponse<TherapistDto>> UpdateAvailability(Guid id)
        {
            var therapist = await _therapistRepository.GetTherapistByUserId(id.ToString());
            if (therapist == null) return new BaseResponse<TherapistDto>
            {
                Message = "therapist not found",
                Status = false,
            };
            if(therapist.IsAvalaible)
            {
                therapist.IsAvalaible = false;
                await _therapistRepository.Update(therapist);
            }
            else
            {
                therapist.IsAvalaible = true;
                await _therapistRepository.Update(therapist);
            }
            return new BaseResponse<TherapistDto>
            {
                Message = "Successful",
                Status = true,
            };

        }
    }
}
