using MindHeal.Implementations.Repositories;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Interfaces.IServices;
using MindHeal.Models.DTOs;

namespace MindHeal.Implementations.Services
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly ISuperAdminRepository _superadminRepository;
        public SuperAdminService(ISuperAdminRepository superadminRepository)
        {
            _superadminRepository = superadminRepository;
        }

        public async Task<BaseResponse<SuperAdminDto>> GetSuperAdmin(Guid id)
        {
            //var superAdmin = await _superadminRepository.GetSuperAdmin(id);
            var superAdmin = await _superadminRepository.GetSuperAdminByUserId(id.ToString());
            if (superAdmin == null) return new BaseResponse<SuperAdminDto>
            {
                Message = "SuperAdmin not found",
                Status = false,
            };

            return new BaseResponse<SuperAdminDto>
            {
                Message = "Successful",
                Status = true,
                Data = new SuperAdminDto
                {
                    Id = superAdmin.Id,
                    FirstName = superAdmin.User.FirstName,
                    LastName = superAdmin.User.LastName,
                    PhoneNumber = superAdmin.User.PhoneNumber,
                    Email = superAdmin.User.Email,
                }
            };
        }
    }
}
