using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Interfaces.IRepositories
{
    public interface IRoleRepository : IBaseRespository
    {
        Task<Role> GetRole(Guid id);
        Task<Role> GetRole(Expression<Func<Role, bool>> expression);
        Task<IEnumerable<Role>> GetAllRoles();
        Role GetRoleByName(string name);
    }
}
