using Microsoft.EntityFrameworkCore;
using MindHeal.Data;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Implementations.Repositories
{
    public class IssuesRepository : BaseRespository, IIssuesRepository
    {
        public IssuesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Issues>> GetAllIssues()
        {
            return await _context.Issues.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<Issues> GetIssues(Guid id)
        {
            return await _context.Issues.Where(x => !x.IsDeleted).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Issues> GetIssues(Expression<Func<Issues, bool>> expression)
        {
            return await _context.Issues.Where(x => !x.IsDeleted).SingleOrDefaultAsync(expression);
        }
    }
}
