using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class UpdateAccountRoleDto
    {
        [Required(ErrorMessage = "Tên role là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên role không được vượt quá 50 ký tự")]
        public string Name { get; set; } = null!;
    }
}
