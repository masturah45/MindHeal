using Microsoft.EntityFrameworkCore;
using MindHeal.Data;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Implementations.Repositories
{
    public class UserRepository : BaseRespository, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users
                .Include(b => b.UserRoles)
                .ThenInclude(c => c.Role)
                .ToListAsync();
        }

        public async Task<User> GetUser(string id)
        {
            return await _context.Users
                .Include(c => c.UserRoles)
                .ThenInclude(c => c.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> Get(Expression<Func<User, bool>> expression)
        {
            var user = _context.Users
                .Include(b => b.Therapist)
                .Include(c => c.Client)
                .Include(d => d.UserRoles)
                .ThenInclude(e => e.Role)
                .FirstOrDefaultAsync(expression);
            return await user;
        }

        public async Task<User> Get(string name)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(x => x.FirstName == name);
        }
    }
}
