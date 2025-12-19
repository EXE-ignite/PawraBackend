namespace Pawra.DAL.Entities
{
    public class AccountRole : BaseEntity
    {
        public string Name { get; set; } = null!;
        // admin, customer, veterinarian, clinic_manager

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }

}