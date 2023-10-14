using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Interfaces.IRepositories
{
    public interface IIssuesRepository : IBaseRespository
    {
        Task<Issues> GetIssues(Guid id);
        Task<Issues> GetIssues(Expression<Func<Issues, bool>> expression);
        Task<IEnumerable<Issues>> GetAllIssues();
    }
}
