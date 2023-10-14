using MindHeal.Models.Entities;
using MindHeal.Models.Entities.Enum;
using System.Linq.Expressions;

namespace MindHeal.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseRespository
    {
        Task<User> GetUser(string id);
        Task<User> Get(string name);
        Task<User> Get(Expression<Func<User, bool>> expression);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
