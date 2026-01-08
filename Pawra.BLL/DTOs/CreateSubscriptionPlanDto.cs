using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreateSubscriptionPlanDto
    {
        [Required(ErrorMessage = "Tên gói đăng ký là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên gói đăng ký không được vượt quá 200 ký tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Thời gian (ngày) là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Thời gian phải lớn hơn 0")]
        public int DurationInDays { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự")]
        public string Description { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
