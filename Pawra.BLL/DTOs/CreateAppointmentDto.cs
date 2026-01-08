using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "PetId là bắt buộc")]
        public Guid PetId { get; set; }

        [Required(ErrorMessage = "CustomerId là bắt buộc")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "VeterinarianId là bắt buộc")]
        public Guid VeterinarianId { get; set; }

        [Required(ErrorMessage = "ClinicId là bắt buộc")]
        public Guid ClinicId { get; set; }

        [Required(ErrorMessage = "Thời gian hẹn là bắt buộc")]
        public DateTime AppointmentTime { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        public string Status { get; set; } = null!;
    }
}
