using Microsoft.EntityFrameworkCore;
using MindHeal.Data;
using MindHeal.Interfaces.IRepositories;
using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Implementations.Repositories
{
    public class TherapistIssuesRepository : BaseRespository, ITherapistIssuesRepository
    {
        public TherapistIssuesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TherapistIssues>> GetAllTherapistIssues()
        {
            return await _context.TherapistIssues.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<TherapistIssues>> GetTherapistByIssues(Guid Issuesid)
        {
            return await _context.TherapistIssues.Where(a => a.IssuesId == Issuesid).Include(a => a.Therapist).ToListAsync();
        }

        public async Task<TherapistIssues> GetTherapistIssues(Guid id)
        {
            return await _context.TherapistIssues.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<TherapistIssues> GetTherapistIssues(Expression<Func<TherapistIssues, bool>> expression)
        {
            return await _context.TherapistIssues.SingleOrDefaultAsync(expression);
        }
    }
}
