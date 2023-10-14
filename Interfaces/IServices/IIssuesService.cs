using MindHeal.Models.DTOs;

namespace MindHeal.Interfaces.IServices
{
    public interface IIssuesService
    {
        Task<BaseResponse<IssuesDto>> Create(CreateIssuesRequestModel model);
        Task<BaseResponse<IssuesDto>> Update(Guid id, UpdateIssuesRequestModel model);
        Task<BaseResponse<IssuesDto>> GetIssues(Guid id);
        Task<BaseResponse<IssuesDto>> Delete(Guid id);
        Task<BaseResponse<IEnumerable<IssuesDto>>> GetAllIssues();
    }
}
