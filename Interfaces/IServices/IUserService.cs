using MindHeal.Models.DTOs;

namespace MindHeal.Interfaces.IServices
{
    public interface IUserService
    {
        Task<BaseResponse<UserDto>> Login(LogInUserRequestModel model);
        Task<BaseResponse<UserDto>> GetUsers(Guid id);
        Task<BaseResponse<IEnumerable<UserDto>>> GetAllUsers();
        Task<BaseResponse<UserDto>> AssignRole(Guid id, List<int> roleIds);
        Task<BaseResponse<UserDto>> AssignTherapistRole(Guid id, string name);
        Task<BaseResponse<UserDto>> Get(string name);
    }
}
