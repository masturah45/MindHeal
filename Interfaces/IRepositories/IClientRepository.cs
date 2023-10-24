using MindHeal.Models.DTOs;
using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Interfaces.IRepositories
{
    public interface IClientRepository : IBaseRespository
    {
        Task<Client> GetClient(Guid id);
        Task<Client> GetClientByUserId(string id);
        Task<Client> GetClientByIdAsync(Guid id);
        Task<Client> GetClient(Expression<Func<Client, bool>> expression);
        Task<IEnumerable<Client>> GetAllClient();
        Task<List<Client>> GetAllClientByChat();
        
    }
}
