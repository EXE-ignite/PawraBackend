using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreatePrescriptionDto
    {
        [Required(ErrorMessage = "AppointmentId là bắt buộc")]
        public Guid AppointmentId { get; set; }

        [Required(ErrorMessage = "Ghi chú là bắt buộc")]
        [StringLength(2000, ErrorMessage = "Ghi chú không được vượt quá 2000 ký tự")]
        public string Notes { get; set; } = null!;
    }
}
