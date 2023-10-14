using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Interfaces.IRepositories
{
    public interface ITherapistIssuesRepository : IBaseRespository
    {
        Task<TherapistIssues> GetTherapistIssues(Guid id);
        Task<TherapistIssues> GetTherapistIssues(Expression<Func<TherapistIssues, bool>> expression);
        Task<IEnumerable<TherapistIssues>> GetAllTherapistIssues();
        Task<IEnumerable<TherapistIssues>> GetTherapistByIssues(Guid Issuesid);
    }
}
