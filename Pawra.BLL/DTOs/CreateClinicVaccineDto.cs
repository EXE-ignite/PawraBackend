using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreateClinicVaccineDto
    {
        [Required(ErrorMessage = "ClinicId là bắt buộc")]
        public Guid ClinicId { get; set; }

        [Required(ErrorMessage = "VaccineId là bắt buộc")]
        public Guid VaccineId { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; }
    }
}

