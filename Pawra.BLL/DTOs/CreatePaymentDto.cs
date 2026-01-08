using System.ComponentModel.DataAnnotations;
using Pawra.DAL.Enums;

namespace Pawra.BLL.DTOs
{
    public class CreatePaymentDto
    {
        [Required(ErrorMessage = "AccountId là bắt buộc")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "PaymentMethodId là bắt buộc")]
        public Guid PaymentMethodId { get; set; }

        [Required(ErrorMessage = "Số tiền là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Ngày thanh toán là bắt buộc")]
        public DateTime PaymentDate { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.pending;
    }
}
