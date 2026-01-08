using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreateServiceDto
    {
        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên dịch vụ không được vượt quá 200 ký tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
        public string Description { get; set; } = null!;
    }
}
