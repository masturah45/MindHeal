using Microsoft.EntityFrameworkCore;
using MindHeal.Models.Entities.Enum;
using MindHeal.Models.Entities;
using System.Linq.Expressions;
using MindHeal.Interfaces.IRepositories;

namespace MindHeal.Implementations.Repositories
{
    public class TherapistRepository : BaseRespository, ITherapistRepository
    {
        public async Task<Therapist> CheckIfExist(string email)
        {
            return await _context.Therapists.Where(e => e.User.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Therapist>> GetAllAvailableTherapist()
        {
            return await _context.Therapists.Where(x => x.IsAvalaible == true).ToListAsync();
        }

        public async Task<IEnumerable<Therapist>> GetAllTherapist()
        {
            return await _context.Therapists
                .Include(x => x.Issues)
                .Include(a => a.User).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Therapist>> GetAllTherapistByChat()
        {
            return await _context.Therapists.Include(a => a.User).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Therapist>> GetAllUnApprovedTherapist()
        {
            return await _context.Therapists.Where(x => x.Status == Approval.Pending && !x.IsDeleted).Include(a => a.User).ToListAsync();
        }

        public async Task<IEnumerable<Therapist>> GetApprovedTherapist()
        {
            return await _context.Therapists.Where(x => x.Status == Approval.Approved && !x.IsDeleted).Include(a => a.User).ToListAsync();
        }

        public async Task<IEnumerable<Therapist>> GetRejectedTherapist()
        {
            return await _context.Therapists.Where(x => x.Status == Approval.Rejected && !x.IsDeleted).Include(a => a.User).ToListAsync();
        }

        public async Task<Therapist> GetTherapist(Guid id)
        {
            return await _context.Therapists.Include(a => a.User).Where(x => !x.IsDeleted).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Therapist> GetTherapist(Expression<Func<Therapist, bool>> expression)
        {
            return await _context.Therapists.Include(a => a.User).Where(x => !x.IsDeleted).FirstOrDefaultAsync(expression);
        }
    }
}
