using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreateVeterinarianDto
    {
        [Required(ErrorMessage = "AccountId là bắt buộc")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "ClinicId là bắt buộc")]
        public Guid ClinicId { get; set; }

        [Required(ErrorMessage = "Số giấy phép là bắt buộc")]
        [StringLength(100, ErrorMessage = "Số giấy phép không được vượt quá 100 ký tự")]
        public string LicenseNumber { get; set; } = null!;
    }
}
