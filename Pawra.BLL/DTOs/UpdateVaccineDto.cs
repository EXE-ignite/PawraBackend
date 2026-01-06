using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class UpdateVaccineDto
    {
        [Required(ErrorMessage = "Tên vaccine là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên vaccine không được vượt quá 200 ký tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Nhà sản xuất là bắt buộc")]
        [StringLength(200, ErrorMessage = "Nhà sản xuất không được vượt quá 200 ký tự")]
        public string Manufacturer { get; set; } = null!;
    }
}

