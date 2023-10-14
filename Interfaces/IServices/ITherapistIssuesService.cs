using MindHeal.Models.DTOs;

namespace MindHeal.Interfaces.IServices
{
    public interface ITherapistIssuesService
    {
        Task<IEnumerable<TherapistIssuesDto>> GetAll();
        Task<BaseResponse<IEnumerable<TherapistDto>>> GetTherapistByIssues(Guid IssuesId);
        Task<BaseResponse<TherapistIssuesDto>> GetTherapistIssues(Guid id);
    }
}
