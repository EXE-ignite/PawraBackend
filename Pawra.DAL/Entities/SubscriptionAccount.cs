using Pawra.DAL.Enums;

namespace Pawra.DAL.Entities
{
    public class SubscriptionAccount : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public Guid SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public SubcriptionStatus Status { get; set; }
        // Active = 0, Expired = 1, Cancelled = 2
    }

}