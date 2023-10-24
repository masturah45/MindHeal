using MindHeal.Models.DTOs;

namespace MindHeal.Interfaces.IServices
{
    public interface ISuperAdminService
    {
        Task<BaseResponse<SuperAdminDto>> GetSuperAdmin(Guid id);
    }
}
