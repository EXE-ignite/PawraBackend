using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreatePrescriptionDto
    {
        [Required(ErrorMessage = "AppointmentId là bắt buộc")]
        public Guid AppointmentId { get; set; }

        [Required(ErrorMessage = "Ghi chú là bắt buộc")]
        public string Notes { get; set; } = null!;
    }
}

