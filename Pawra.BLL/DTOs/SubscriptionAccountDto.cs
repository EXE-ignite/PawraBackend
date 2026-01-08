using Pawra.DAL.Enums;

namespace Pawra.BLL.DTOs
{
    public class SubscriptionAccountDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubcriptionStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

