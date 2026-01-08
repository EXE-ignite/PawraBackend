using System.ComponentModel.DataAnnotations;
using Pawra.DAL.Enums;

namespace Pawra.BLL.DTOs
{
    public class UpdateSubscriptionAccountDto
    {
        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        public SubcriptionStatus Status { get; set; }
    }
}
