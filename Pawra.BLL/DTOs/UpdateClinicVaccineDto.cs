using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class UpdateClinicVaccineDto
    {
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; }
    }
}
