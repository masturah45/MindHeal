using Microsoft.AspNetCore.Identity;
using MindHeal.Implementations.Repositories;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;
using MindHeal.Models.Entities.Enum;
using System.Text.RegularExpressions;

namespace MindHeal.Implementations.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationMessage _notificationMessage;
        private readonly UserManager<User> _userManager;
        private readonly IRoleRepository _roleRepository;
        public ClientService(IClientRepository clientRepository, IUserRepository userRepository, INotificationMessage notificationMessage, UserManager<User> userManager, IRoleRepository roleRepository)
        {
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _notificationMessage = notificationMessage;
            _userManager = userManager;
            _roleRepository = roleRepository;
        }

        private static bool ValidatePassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[A-Z])(?=.*[@#$%^&+=])(?=.{8,})");
            return regex.IsMatch(password);
        }
        public async Task<BaseResponse<ClientDto>> Create(CreateClientRequestModel model)
        {
            bool isValid = ValidatePassword(model.Password);
            if (!isValid)
            {
                return new BaseResponse<ClientDto>
                {
                    Message = "Password is invalid. Password must be at least 8 characters long, contain at least one capital letter, and a special character.",
                    Status = false,

                };
            }

            var checkIfExist = await _clientRepository.CheckIfExist(model.Email);
            if (checkIfExist != null) return new BaseResponse<ClientDto>
            {
                Message = "User already exist",
                Status = false,
            };

            var role = await _roleRepository.Get<Role>(a => a.Name == "Client");

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                UserRoles = new List<UserRole>()
            };

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = Guid.NewGuid(),
                Role = role,
                User = user,
            };
            user.UserRoles.Add(userRole);
            await _userManager.CreateAsync(user, model.Password);
            var client = new Client
            {
                User = user,
                UserId = userRole.UserId.ToString(),
                State = model.State,
                DateOfBirth = model.DateOfBirth,

            };
            await _clientRepository.Add(client);
            await _clientRepository.save();
            var request = new WhatsappMessageSenderRequestModel { ReciprantNumber = model.PhoneNumber, MessageBody = "Client created successfully" };
            await _notificationMessage.SendWhatsappMessageAsync(request);
            return new BaseResponse<ClientDto>
            {
                Message = "Client created successfully",
                Status = true,
                Data = new ClientDto
                {
                    FirstName = client.User.FirstName,
                    LastName = client.User.LastName,
                    Email = client.User.Email,
                    Gender = client.User.Gender,
                }
            };

        }

        public async Task<BaseResponse<ClientDto>> Delete(Guid id)
        {

            var client = await _clientRepository.Get<Client>(id);
            if (client == null) return new BaseResponse<ClientDto>
            {
                Message = "Client Not Found",
                Status = false,
            };

            client.IsDeleted = true;
            await _clientRepository.save();

            return new BaseResponse<ClientDto>
            {
                Message = "Deleted Successfully",
                Status = true,
            };
        }

        public async Task<IEnumerable<ClientDto>> GetAll()
        {
            var clients = await _clientRepository.GetAllClient();
            var listOfClients = clients.Select(a => new ClientDto
            {
                Id = a.Id,
                UserId = a.UserId,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                Email = a.User.Email,
                PhoneNumber = a.User.PhoneNumber,
                DateOfBirth = a.DateOfBirth,
                State = a.State,
            }).ToList();
            return listOfClients;
        }

        public async Task<List<UserDto>> GetAllClientByChat()
        {
            var clients = await _clientRepository.GetAllClient();
            var listOfClients = clients.Select(a => new UserDto
            {
                Id = a.UserId,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
            }).ToList();
            return listOfClients;
        }

        public async Task<BaseResponse<ClientDto>> GetClient(Guid id)
        {
            var client = await _clientRepository.GetClient(id);
            if (client == null) return new BaseResponse<ClientDto>
            {
                Message = "Client not found",
                Status = false,
            };

            return new BaseResponse<ClientDto>
            {
                Message = "Successful",
                Status = true,
                Data = new ClientDto
                {
                    Id = client.Id,
                    FirstName = client.User.FirstName,
                    LastName = client.User.LastName,
                    PhoneNumber = client.User.PhoneNumber,
                    Email = client.User.Email,
                    DateOfBirth = client.DateOfBirth,
                    State = client.State,
                    Gender = client.User.Gender,
                }
            };
        }

        public async Task<BaseResponse<ClientDto>> Update(Guid id, UpdateClientRequestModel model)
        {
            var request = new WhatsappMessageSenderRequestModel { ReciprantNumber = model.PhoneNumber, MessageBody = "Client edited successfully" };
            await _notificationMessage.SendWhatsappMessageAsync(request);

            var client = await _clientRepository.GetClient(id);
            if (client == null) return new BaseResponse<ClientDto>
            {
                Message = "client not found",
                Status = false,
            };

            client.User.FirstName = model.FirstName;
            client.User.LastName = model.LastName;
            client.User.Email = model.Email;
            client.User.PhoneNumber = model.PhoneNumber;
            client.DateOfBirth = model.DateOfBirth;
            client.State = model.State;
            client.DateCreated = DateTime.Now;
            client.DateUpdated = DateTime.Now;
            client.IsDeleted = false;

            await _clientRepository.Update(client);

            return new BaseResponse<ClientDto>
            {
                Message = "Successful",
                Status = true,
                Data = new ClientDto
                {
                    FirstName = client.User.FirstName,
                    LastName = client.User.LastName,
                }
            };
        }
    }
}
