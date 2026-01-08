using System.ComponentModel.DataAnnotations;
using Pawra.DAL.Enums;

namespace Pawra.BLL.DTOs
{
    public class UpdatePaymentDto
    {
        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        public PaymentStatus Status { get; set; }
    }
}

