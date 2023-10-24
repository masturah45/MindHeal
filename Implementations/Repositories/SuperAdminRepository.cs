using Microsoft.EntityFrameworkCore;
using MindHeal.Data;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Implementations.Repositories
{
    public class SuperAdminRepository : BaseRespository, ISuperAdminRepository
    {
        public SuperAdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<SuperAdmin> GetSuperAdmin(Guid id)
        {
            return await _context.SuperAdmins.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<SuperAdmin> GetSuperAdmin(Expression<Func<SuperAdmin, bool>> expression)
        {
            return await _context.SuperAdmins.Include(a => a.User).FirstOrDefaultAsync(expression);
        }

        public async Task<SuperAdmin> GetSuperAdminByUserId(string id)
        {
            return await _context.SuperAdmins.Include(a => a.User).Where(x => !x.IsDeleted).FirstOrDefaultAsync(a => a.UserId == id);
        }
    }
}
