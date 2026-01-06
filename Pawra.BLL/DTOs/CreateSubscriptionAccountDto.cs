using System.ComponentModel.DataAnnotations;
using Pawra.DAL.Enums;

namespace Pawra.BLL.DTOs
{
    public class CreateSubscriptionAccountDto
    {
        [Required(ErrorMessage = "AccountId là bắt buộc")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "SubscriptionPlanId là bắt buộc")]
        public Guid SubscriptionPlanId { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        public SubcriptionStatus Status { get; set; }
    }
}

