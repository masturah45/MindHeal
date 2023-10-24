using MindHeal.Models.DTOs;

namespace MindHeal.Interfaces.IServices
{
    public interface ITherapistService
    {
        Task<BaseResponse<TherapistDto>> Create(CreateTherapistRequestModel model);
        Task<BaseResponse<TherapistDto>> Update(Guid id, UpdateTherapistRequestModel model);
        Task<BaseResponse<TherapistDto>> GetTherapist(Guid id);
        Task<IEnumerable<TherapistDto>> GetAll();
        Task<BaseResponse<TherapistDto>> UpdateAvailability(Guid id);
        Task<BaseResponse<TherapistDto>> Delete(Guid id);
        Task<BaseResponse<IEnumerable<TherapistDto>>> ViewUnapprovedTherapist();
        Task<BaseResponse<IEnumerable<TherapistDto>>> ViewapprovedTherapist();
        Task<BaseResponse<IEnumerable<TherapistDto>>> ViewRejectedTherapist();
        Task<List<UserDto>> GetAllTherapistByChat();
        Task<IEnumerable<TherapistDto>> GetAllAvailableTherapist();
        Task<BaseResponse<TherapistDto>> Approve(Guid id);
        Task<BaseResponse<TherapistDto>> RejectapprovedTherapist(Guid id);
        Task<BaseResponse<TherapistDto>> GetTherapistForProfile(string id);
    }
}
