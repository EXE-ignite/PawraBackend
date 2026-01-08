using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class UpdateClinicDto
    {
        [Required(ErrorMessage = "Tên phòng khám là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên phòng khám không được vượt quá 200 ký tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "ClinicManagerId là bắt buộc")]
        public Guid ClinicManagerId { get; set; }
    }
}

