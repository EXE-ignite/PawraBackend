using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password là bắt buộc")]
        public string Password { get; set; } = null!;
    }
}
