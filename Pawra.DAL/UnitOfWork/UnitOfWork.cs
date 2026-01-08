using Pawra.DAL.Interfaces;
using Pawra.DAL.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using Pawra.DAL.Entities;

namespace Pawra.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PawraDBContext _dbContext;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _transaction;
        private IAccountRoleRepository? _accountRoleRepository;

        public UnitOfWork(PawraDBContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, object>();
        }

        public IAccountRoleRepository AccountRoleRepository
        {
            get
            {
                _accountRoleRepository ??= new AccountRoleRepository(_dbContext);
                return _accountRoleRepository;
            }
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T);
            
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = Activator.CreateInstance(typeof(BaseRepository<T>), _dbContext);
                _repositories.Add(type, repositoryInstance!);
            }

            return (IRepository<T>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                }
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                }
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
