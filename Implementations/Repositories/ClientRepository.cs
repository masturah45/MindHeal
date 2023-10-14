using Microsoft.EntityFrameworkCore;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Implementations.Repositories
{
    public class ClientRepository : BaseRespository, IClientRepository
    {
        public async Task<Client> CheckIfExist(string email)
        {
            return await _context.Clients.Where(e => e.User.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Client>> GetAllClient()
        {
            return await _context.Clients.Include(a => a.User).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Client>> GetAllClientByChat()
        {
            return await _context.Clients.Include(a => a.User).Where(x => !x.IsDeleted).ToListAsync();
        }
    

        public async Task<Client> GetClient(Guid id)
        {
            return await _context.Clients.Include(a => a.User).Where(x => !x.IsDeleted).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Client> GetClient(Expression<Func<Client, bool>> expression)
        {
            return await _context.Clients.Include(a => a.User).Where(x => !x.IsDeleted).FirstOrDefaultAsync(expression);
        }

        public async Task<Client> GetClientByIdAsync(Guid id)
        {
            return await _context.Clients.Include(a => a.User).Where(x => !x.IsDeleted).FirstOrDefaultAsync(a => a.UserId == id.ToString());
        }
    }
}
