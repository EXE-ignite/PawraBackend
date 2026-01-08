using Pawra.DAL.Enums;

namespace Pawra.BLL.DTOs
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

