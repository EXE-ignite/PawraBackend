using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreateClinicServiceDto
    {
        [Required(ErrorMessage = "ClinicId là bắt buộc")]
        public Guid ClinicId { get; set; }

        [Required(ErrorMessage = "ServiceId là bắt buộc")]
        public Guid ServiceId { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; }
    }
}
