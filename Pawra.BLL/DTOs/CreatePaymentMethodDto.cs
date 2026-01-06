using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreatePaymentMethodDto
    {
        [Required(ErrorMessage = "Tên phương thức thanh toán là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên phương thức thanh toán không được vượt quá 100 ký tự")]
        public string Name { get; set; } = null!;
    }
}

