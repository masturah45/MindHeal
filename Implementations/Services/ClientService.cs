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
        //private readonly RoleManager<string> _roleManager;
        private readonly IClientRepository _clientRepository;
        private readonly INotificationMessage _notificationMessage;
        private readonly UserManager<User> _userManager;
        public ClientService(IClientRepository clientRepository,  INotificationMessage notificationMessage, UserManager<User> userManager)
        {
            _clientRepository = clientRepository;
            _notificationMessage = notificationMessage;
            _userManager = userManager;
            //_roleManager = roleManager;
        }

        private static bool ValidatePassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[A-Z])(?=.*[@#$%^&+=])(?=.{8,})");
            return regex.IsMatch(password);
        }
        public async Task<BaseResponse<ClientDto>> Create(CreateClientRequestModel model)
        {
            try
            {

            var checkIfExist = await _userManager.FindByEmailAsync(model.Email);
            if (checkIfExist != null) 
                return new BaseResponse<ClientDto>
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
                UserName = model.Email,
                Gender = model.Gender,
            };
           var createdUser =  await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Client");
            var client = new Client
            {
                User = user,
                UserId = user.Id,
                State = model.State,
                DateOfBirth = model.DateOfBirth,

            };
            await _clientRepository.Add(client);
            var userr = await _userManager.FindByIdAsync(client.UserId);
            userr.EmailConfirmed = true;
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new BaseResponse<ClientDto>
                {
                    Message = "An error occurred while creating the client",
                    Status = false,
                };
            }

        }

        public async Task<BaseResponse<ClientDto>> Delete(Guid id)
        {

            var client = await _clientRepository.GetClientByUserId(id.ToString());
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
            var user = await _clientRepository.GetClientByUserId(client.UserId);
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
                    Id = user.Id,
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName,
                    PhoneNumber = user.User.PhoneNumber,
                    Email = user.User.Email,
                    DateOfBirth = user.DateOfBirth,
                    State = user.State,
                    Gender = user.User.Gender,
                    UserId = user.UserId,
                }
            };
        }

        public async Task<BaseResponse<ClientDto>> GetClientForProfile(string id)
        {
            //var client = await _clientRepository.GetClient(id);
            var user = await _clientRepository.GetClientByUserId(id);
            if (user == null) return new BaseResponse<ClientDto>
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
                    Id = user.Id,
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName,
                    PhoneNumber = user.User.PhoneNumber,
                    Email = user.User.Email,
                    DateOfBirth = user.DateOfBirth,
                    State = user.State,
                    Gender = user.User.Gender,
                    UserId = user.UserId,
                }
            };
        }

        public async Task<BaseResponse<ClientDto>> Update(Guid id, UpdateClientRequestModel model)
        {
            var request = new WhatsappMessageSenderRequestModel { ReciprantNumber = model.PhoneNumber, MessageBody = "Client edited successfully" };
            await _notificationMessage.SendWhatsappMessageAsync(request);

            //var client = await _clientRepository.GetClient(id);
            var client = await _clientRepository.GetClientByUserId(id.ToString());
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
