using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using MindHeal.FileManagers;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;
using MindHeal.Models.Entities.Enum;

namespace MindHeal.Implementations.Services
{
    public class UserService : IUserService
    {
        //private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        //private readonly IRoleRepository _roleRepository;

        public UserService(UserManager<User> userManager)
        {
            //_userRepository = userRepository;
            _userManager = userManager;
            //_roleRepository = roleRepository;
        }

        public async Task<BaseResponse<UserDto>> AssignRole(Guid id, List<int> roleIds)
        {
            //var user = await _userRepository.GetUser(id.ToString());
            //if (user == null) return new BaseResponse<UserDto>
            //{
            //    Message = "user not found",
            //    Status = false,
            //};
            return new BaseResponse<UserDto>
            {
                Status = true,
                Message = "Successfully",
            };
        }

        public async Task<BaseResponse<UserDto>> AssignTherapistRole(Guid id, string name)
        {
            UserDto user = null;// await _userRepository.GetUser(id.ToString());
            if (user == null) return new BaseResponse<UserDto>
            {
                Message = "user not found",
                Status = false,
            };

            //var role = await _roleRepository.Get<Role>(b => b.Name == "Therapist");

            //user.UserRoles.Add(new UserRole
            //{
            //    RoleId = Guid.NewGuid(),
            //    UserId = user.Id
            //});

            return new BaseResponse<UserDto>
            {
                Status = true,
                Message = "Successfully",
            };
        }

        public async Task<BaseResponse<UserDto>> Get(string name)
        {
            //var user = await _userRepository.Get(name);
            //if (user == null) return new BaseResponse<UserDto>
            //{
            //    Message = "User not found",
            //    Status = false,
            //};
            return new BaseResponse<UserDto>
            {
                Status = true,
                Message = "Successfully",
                Data = new UserDto
                {
                    //Id = user.Id,
                    ////Id = Guid.NewGuid(),
                    //FirstName = user.FirstName,
                    //LastName = user.LastName,
                    //Email = user.Email,

                    //Roles = user.UserRoles.Select(a => new RoleDto
                    //{
                    //    Id = Guid.Parse(user.Id),
                    //    //Id = Guid.NewGuid(),
                    //    Name = a.Role.Name,
                    //    Description = a.Role.Description,
                    //}).ToList(),
                }
            };
        }

        public async Task<BaseResponse<IEnumerable<UserDto>>> GetAllUsers()
        {
            //var users = await _userRepository.GetAllUsers();
            //var listOfUsers = users.ToList().Select(user => new UserDto
            //{
            //    Id = user.Id,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Email = user.Email,
            //    PhoneNumber = user.PhoneNumber,
            //});

            return new BaseResponse<IEnumerable<UserDto>>
            {
                Message = "success",
                Status = true,
                //Data = listOfUsers,
            };
        }

        public async Task<BaseResponse<UserDto>> GetUsers(Guid id)
        {
            //var user = await _userRepository.GetUser(id.ToString());
            //if (user == null) return new BaseResponse<UserDto>
            //{
            //    Message = "User not found",
            //    Status = false,
            //};

            return new BaseResponse<UserDto>
            {
                Status = true,
                Message = "Successfully",
                Data = new UserDto
                {
                    //Id = user.Id,
                    //FirstName = user.FirstName,
                    //LastName = user.LastName,
                    //Email = user.Email,

                    //Roles = user.UserRoles.Select(a => new RoleDto
                    //{
                    //    Id = Guid.Parse(user.Id),
                    //    Name = a.Role.Name,
                    //    Description = a.Role.Description,
                    //}).ToList(),
                }
            };
        }

        public async Task<BaseResponse<UserDto>> Login(LogInUserRequestModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new BaseResponse<UserDto>
                {
                    Message = "incorect detail",
                    Status = false,
                };
            var userlogin = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!userlogin)
                return new BaseResponse<UserDto>
                {
                    Message = "incorect detail",
                    Status = false,
                };

            return new BaseResponse<UserDto>
            {
                Message = "login successful",
                Status = true,
                Data = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                }
            };
        }
    }
}
