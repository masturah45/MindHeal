using Microsoft.AspNetCore.Identity;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;

namespace MindHeal.Implementations.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<BaseResponse<RoleDto>> Create(CreateRoleRequestModel model)
        {
            var roleExist = await _roleRepository.Get<Role>(b => b.Name == model.Name);
            if (roleExist != null) return new BaseResponse<RoleDto>
            {
                Message = "Role already exist",
                Status = false,
            };

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                IsDeleted = false,
            };

            await _roleRepository.Add(role);

            return new BaseResponse<RoleDto>
            {
                Status = true,
                Message = "Created Successfully",
                Data = new RoleDto
                {
                    Name = model.Name
                }
            };
        }

        public async Task<BaseResponse<RoleDto>> GetRoleByName(string name)
        {
            var role = _roleRepository.GetRoleByName(name);
            if (role == null) return new BaseResponse<RoleDto>
            {
                Message = "role not found",
                Status = false,
            };
            return new BaseResponse<RoleDto>
            {
                Message = "Successfully",
                Status = true,
                Data = new RoleDto
                {
                    Id = Guid.NewGuid(),
                    Name = role.Name,
                    Description = role.Description,
                }
            };
        }
    }
}
