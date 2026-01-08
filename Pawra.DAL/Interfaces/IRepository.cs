using Pawra.DAL.Entities;
using System.Linq.Expressions;

namespace Pawra.DAL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        // Modern async methods
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<int> SaveChangesAsync();

        // Legacy methods for BaseService compatibility
        Task<T> Create(T entity);
        Task<IEnumerable<T>> Read(Expression<Func<T, bool>> filter = null, 
                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                  int pageNumber = 1, 
                                  int pageSize = 100);
        Task<T?> Read(object id);
        Task Update(T entity);
        Task Delete(object id);
    }
}
