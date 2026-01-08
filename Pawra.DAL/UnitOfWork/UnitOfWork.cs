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
        private IClinicRepository? _clinicRepository;
        private ICustomerRepository? _customerRepository;
        private IPetRepository? _petRepository;
        private IVaccineRepository? _vaccineRepository;
        private IServiceRepository? _serviceRepository;
        private IPaymentMethodRepository? _paymentMethodRepository;
        private ISubscriptionPlanRepository? _subscriptionPlanRepository;
        private IAppointmentRepository? _appointmentRepository;
        private IVeterinarianRepository? _veterinarianRepository;
        private IClinicManagerRepository? _clinicManagerRepository;
        private IClinicServiceRepository? _clinicServiceRepository;
        private IClinicVaccineRepository? _clinicVaccineRepository;
        private IPaymentRepository? _paymentRepository;
        private IPrescriptionRepository? _prescriptionRepository;
        private ISubscriptionAccountRepository? _subscriptionAccountRepository;
        private IVaccinationRecordRepository? _vaccinationRecordRepository;

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

        public IClinicRepository ClinicRepository
        {
            get
            {
                _clinicRepository ??= new ClinicRepository(_dbContext);
                return _clinicRepository;
            }
        }

        public ICustomerRepository CustomerRepository
        {
            get
            {
                _customerRepository ??= new CustomerRepository(_dbContext);
                return _customerRepository;
            }
        }

        public IPetRepository PetRepository
        {
            get
            {
                _petRepository ??= new PetRepository(_dbContext);
                return _petRepository;
            }
        }

        public IVaccineRepository VaccineRepository
        {
            get
            {
                _vaccineRepository ??= new VaccineRepository(_dbContext);
                return _vaccineRepository;
            }
        }

        public IServiceRepository ServiceRepository
        {
            get
            {
                _serviceRepository ??= new ServiceRepository(_dbContext);
                return _serviceRepository;
            }
        }

        public IPaymentMethodRepository PaymentMethodRepository
        {
            get
            {
                _paymentMethodRepository ??= new PaymentMethodRepository(_dbContext);
                return _paymentMethodRepository;
            }
        }

        public ISubscriptionPlanRepository SubscriptionPlanRepository
        {
            get
            {
                _subscriptionPlanRepository ??= new SubscriptionPlanRepository(_dbContext);
                return _subscriptionPlanRepository;
            }
        }

        public IAppointmentRepository AppointmentRepository
        {
            get
            {
                _appointmentRepository ??= new AppointmentRepository(_dbContext);
                return _appointmentRepository;
            }
        }

        public IVeterinarianRepository VeterinarianRepository
        {
            get
            {
                _veterinarianRepository ??= new VeterinarianRepository(_dbContext);
                return _veterinarianRepository;
            }
        }

        public IClinicManagerRepository ClinicManagerRepository
        {
            get
            {
                _clinicManagerRepository ??= new ClinicManagerRepository(_dbContext);
                return _clinicManagerRepository;
            }
        }

        public IClinicServiceRepository ClinicServiceRepository
        {
            get
            {
                _clinicServiceRepository ??= new ClinicServiceRepository(_dbContext);
                return _clinicServiceRepository;
            }
        }

        public IClinicVaccineRepository ClinicVaccineRepository
        {
            get
            {
                _clinicVaccineRepository ??= new ClinicVaccineRepository(_dbContext);
                return _clinicVaccineRepository;
            }
        }

        public IPaymentRepository PaymentRepository
        {
            get
            {
                _paymentRepository ??= new PaymentRepository(_dbContext);
                return _paymentRepository;
            }
        }

        public IPrescriptionRepository PrescriptionRepository
        {
            get
            {
                _prescriptionRepository ??= new PrescriptionRepository(_dbContext);
                return _prescriptionRepository;
            }
        }

        public ISubscriptionAccountRepository SubscriptionAccountRepository
        {
            get
            {
                _subscriptionAccountRepository ??= new SubscriptionAccountRepository(_dbContext);
                return _subscriptionAccountRepository;
            }
        }

        public IVaccinationRecordRepository VaccinationRecordRepository
        {
            get
            {
                _vaccinationRecordRepository ??= new VaccinationRecordRepository(_dbContext);
                return _vaccinationRecordRepository;
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
