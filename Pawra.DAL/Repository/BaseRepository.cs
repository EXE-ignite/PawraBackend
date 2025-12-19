using Microsoft.EntityFrameworkCore;
using Pawra.DAL.Interfaces;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Pawra.DAL.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal PawraDBContext dbContext;
        internal DbSet<T> dbSet;

        public BaseRepository(PawraDBContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = this.dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> Read([Optional] Expression<Func<T, bool>> filter,
                                               [Optional] Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
                                               int pageNumber = 1,
                                               int pageSize = 100)
        {
            IQueryable<T> query = dbSet;

            //Searching/Filtering
            if (filter != null)
                query = query.AsNoTracking().Where(filter);

            //Sorting
            if (orderBy != null)
                return orderBy(query);

            //Paging
            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public virtual async Task<T?> Read(object id)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<T>(e, "Id").Equals(id));
        }

        // IRepository implementation
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<T>(e, "Id").Equals(id));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
        {
            return await Task.FromResult(dbSet.AsNoTracking().Where(predicate).AsEnumerable());
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        // Legacy methods (keep for backward compatibility)
        public virtual async Task<T> Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task Update(T entity)
        {
            dbSet.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(object id)
        {
            var entity = await Read(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
