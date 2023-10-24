using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Interfaces.IRepositories
{
    public interface ISuperAdminRepository : IBaseRespository
    {
        Task<SuperAdmin> GetSuperAdmin(Guid id);
        Task<SuperAdmin> GetSuperAdminByUserId(string id);
        Task<SuperAdmin> GetSuperAdmin(Expression<Func<SuperAdmin, bool>> expression);
    }
}
