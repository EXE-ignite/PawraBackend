namespace Pawra.DAL.Entities
{
    public class SubscriptionPlan : BaseEntity
    {
        public string Name { get; set; } = null!; 
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }

        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }

        public ICollection<SubscriptionAccount> SubscriptionAccounts { get; set; }
            = new List<SubscriptionAccount>();
    }

}