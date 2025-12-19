namespace Pawra.DAL.Entities
{
    public class Account :BaseEntity
    {
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public Guid RoleId { get; set; }
        public AccountRole Role { get; set; } = null!;

        public ICollection<SubscriptionAccount> SubscriptionAccounts { get; set; } = new List<SubscriptionAccount>();

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public Customer? Customer { get; set; }
        public Veterinarian? Veterinarian { get; set; }
        public ClinicManager? ClinicManager { get; set; }
    }
}
