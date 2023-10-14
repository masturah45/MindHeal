using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Interfaces.IRepositories
{
    public interface ITherapistRepository : IBaseRespository
    {
        Task<Therapist> GetTherapist(Guid id);
        Task<Therapist> CheckIfExist(string email);
        Task<Therapist> GetTherapist(Expression<Func<Therapist, bool>> expression);
        Task<IEnumerable<Therapist>> GetAllTherapist();
        Task<IEnumerable<Therapist>> GetAllUnApprovedTherapist();
        Task<IEnumerable<Therapist>> GetApprovedTherapist();
        Task<IEnumerable<Therapist>> GetRejectedTherapist();
        Task<IEnumerable<Therapist>> GetAllAvailableTherapist();
        Task<List<Therapist>> GetAllTherapistByChat();
    }
}
