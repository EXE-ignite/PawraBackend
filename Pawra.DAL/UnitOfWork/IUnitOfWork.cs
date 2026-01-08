using Pawra.DAL.Entities;

namespace Pawra.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : BaseEntity;
        IAccountRoleRepository AccountRoleRepository { get; }
        IClinicRepository ClinicRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IPetRepository PetRepository { get; }
        IVaccineRepository VaccineRepository { get; }
        IServiceRepository ServiceRepository { get; }
        IPaymentMethodRepository PaymentMethodRepository { get; }
        ISubscriptionPlanRepository SubscriptionPlanRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }
        IVeterinarianRepository VeterinarianRepository { get; }
        IClinicManagerRepository ClinicManagerRepository { get; }
        IClinicServiceRepository ClinicServiceRepository { get; }
        IClinicVaccineRepository ClinicVaccineRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IPrescriptionRepository PrescriptionRepository { get; }
        ISubscriptionAccountRepository SubscriptionAccountRepository { get; }
        IVaccinationRecordRepository VaccinationRecordRepository { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
