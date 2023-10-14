using MindHeal.Models.DTOs;

namespace MindHeal.Interfaces.IServices
{
    public interface IRoleService
    {
        Task<BaseResponse<RoleDto>> Create(CreateRoleRequestModel model);
        Task<BaseResponse<RoleDto>> GetRoleByName(string name);
    }
}
