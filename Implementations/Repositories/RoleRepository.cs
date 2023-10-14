using Microsoft.EntityFrameworkCore;
using MindHeal.Data;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Implementations.Repositories
{
    public class RoleRepository : BaseRespository, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _context.Roles
               .Include(r => r.UserRoles)
               .ThenInclude(r => r.User)
                .ToListAsync();
        }

        public async Task<Role> GetRole(Guid id)
        {
            return await _context.Roles
            .Include(r => r.UserRoles)
            .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Role> GetRole(Expression<Func<Role, bool>> expression)
        {
            return await _context.Roles.Include(x => x.UserRoles).ThenInclude(x => x.User).FirstOrDefaultAsync(expression);
        }

        public Role GetRoleByName(string name)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Name == name);
            return role;
        }
    }
}
