using Microsoft.EntityFrameworkCore;
using Pawra.DAL.Entities;

namespace Pawra.DAL
{
    public class PawraDBContext : DbContext
    {
        public PawraDBContext(DbContextOptions<PawraDBContext> options) : base(options)
        {
        }

        // Entities
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicManager> ClinicManagers { get; set; }
        public DbSet<ClinicService> ClinicServices { get; set; }
        public DbSet<ClinicVaccine> ClinicVaccines { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SubscriptionAccount> SubscriptionAccounts { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<VaccinationRecord> VaccinationRecords { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<Veterinarian> Veterinarians { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Account - AccountRole (One-to-Many)
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(a => a.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Account - Customer (One-to-One)
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Account)
                .WithOne(a => a.Customer)
                .HasForeignKey<Customer>(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Account - Veterinarian (One-to-One)
            modelBuilder.Entity<Veterinarian>()
                .HasOne(v => v.Account)
                .WithOne(a => a.Veterinarian)
                .HasForeignKey<Veterinarian>(v => v.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Account - ClinicManager (One-to-One)
            modelBuilder.Entity<ClinicManager>()
                .HasOne(cm => cm.Account)
                .WithOne()
                .HasForeignKey<ClinicManager>(cm => cm.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Account - SubscriptionAccount (One-to-Many)
            modelBuilder.Entity<SubscriptionAccount>()
                .HasOne(sa => sa.Account)
                .WithMany(a => a.SubscriptionAccounts)
                .HasForeignKey(sa => sa.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Account - Payment (One-to-Many)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Account)
                .WithMany(a => a.Payments)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // ClinicManager - Clinic (One-to-Many)
            modelBuilder.Entity<Clinic>()
                .HasOne(c => c.ClinicManager)
                .WithMany(cm => cm.Clinics)
                .HasForeignKey(c => c.ClinicManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Clinic - Veterinarian (One-to-Many)
            modelBuilder.Entity<Veterinarian>()
                .HasOne(v => v.Clinic)
                .WithMany(c => c.Veterinarians)
                .HasForeignKey(v => v.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            // Clinic - Appointment (One-to-Many)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Clinic)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            // Clinic - ClinicVaccine (One-to-Many)
            modelBuilder.Entity<ClinicVaccine>()
                .HasOne(cv => cv.Clinic)
                .WithMany(c => c.ClinicVaccines)
                .HasForeignKey(cv => cv.ClinicId)
                .OnDelete(DeleteBehavior.Cascade);

            // Clinic - ClinicService (One-to-Many)
            modelBuilder.Entity<ClinicService>()
                .HasOne(cs => cs.Clinic)
                .WithMany(c => c.ClinicServices)
                .HasForeignKey(cs => cs.ClinicId)
                .OnDelete(DeleteBehavior.Cascade);

            // Clinic - VaccinationRecord (One-to-Many)
            modelBuilder.Entity<VaccinationRecord>()
                .HasOne(vr => vr.Clinic)
                .WithMany(c => c.VaccinationRecords)
                .HasForeignKey(vr => vr.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer - Pet (One-to-Many)
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Pets)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer - Appointment (One-to-Many)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Pet - Appointment (One-to-Many)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Pet)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Pet - VaccinationRecord (One-to-Many)
            modelBuilder.Entity<VaccinationRecord>()
                .HasOne(vr => vr.Pet)
                .WithMany(p => p.VaccinationRecords)
                .HasForeignKey(vr => vr.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Veterinarian - Appointment (One-to-Many)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Veterinarian)
                .WithMany(v => v.Appointments)
                .HasForeignKey(a => a.VeterinarianId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - Prescription (One-to-Many)
            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Appointment)
                .WithMany(a => a.Prescription)
                .HasForeignKey(p => p.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // PaymentMethod - Payment (One-to-Many)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentMethod)
                .WithMany(pm => pm.Payments)
                .HasForeignKey(p => p.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            // SubscriptionPlan - SubscriptionAccount (One-to-Many)
            modelBuilder.Entity<SubscriptionAccount>()
                .HasOne(sa => sa.SubscriptionPlan)
                .WithMany(sp => sp.SubscriptionAccounts)
                .HasForeignKey(sa => sa.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Service - ClinicService (One-to-Many)
            modelBuilder.Entity<ClinicService>()
                .HasOne(cs => cs.Service)
                .WithMany(s => s.ClinicServices)
                .HasForeignKey(cs => cs.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vaccine - ClinicVaccine (One-to-Many)
            modelBuilder.Entity<ClinicVaccine>()
                .HasOne(cv => cv.Vaccine)
                .WithMany(v => v.ClinicVaccines)
                .HasForeignKey(cv => cv.VaccineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vaccine - VaccinationRecord (One-to-Many)
            modelBuilder.Entity<VaccinationRecord>()
                .HasOne(vr => vr.Vaccine)
                .WithMany(v => v.VaccinationRecords)
                .HasForeignKey(vr => vr.VaccineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
