using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class UpdateAppointmentDto
    {
        [Required(ErrorMessage = "Thời gian hẹn là bắt buộc")]
        public DateTime AppointmentTime { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        public string Status { get; set; } = null!;
    }
}
