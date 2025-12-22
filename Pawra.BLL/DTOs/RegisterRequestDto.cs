using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password là bắt buộc")]
        [MinLength(6, ErrorMessage = "Password phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "FullName là bắt buộc")]
        public string FullName { get; set; } = null!;
    }
}
