using MindHeal.Models.Entities;
using System.Linq.Expressions;

namespace MindHeal.Interfaces.IRepositories
{
    public interface IBaseRespository
    {
        Task<T> Add<T>(T entity) where T : BaseEntity;
        Task<T> Update<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
        Task<bool> save();
        Task<T> Get<T>(Guid id) where T : BaseEntity;
        Task<T> Get<T>(Expression<Func<T, bool>> expression) where T : BaseEntity;
        Task<IEnumerable<T>> GetAll<T>() where T : BaseEntity;
        IQueryable<T> QueryWhere<T>(Expression<Func<T, bool>> expression) where T : BaseEntity;
    }
}
