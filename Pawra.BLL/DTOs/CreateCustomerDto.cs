using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "AccountId là bắt buộc")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string Phone { get; set; } = null!;
    }
}
