using Pawra.DAL.Enums;

namespace Pawra.DAL.Entities
{
    public class Payment : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public Guid PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = null!;

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public PaymentStatus Status { get; set; }
    }

}