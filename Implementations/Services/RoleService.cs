using Microsoft.AspNetCore.Identity;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;

namespace MindHeal.Implementations.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<BaseResponse<RoleDto>> Create(CreateRoleRequestModel model)
        {
            var roleExist = await _roleManager.FindByNameAsync(model.Name);
            if (roleExist != null) return new BaseResponse<RoleDto>
            {
                Message = "Role already exist",
                Status = false,
            };

            var role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
             
            };

            await _roleManager.CreateAsync(role);

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
    }
}
