using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class UpdatePrescriptionDto
    {
        [Required(ErrorMessage = "Ghi chú là bắt buộc")]
        public string Notes { get; set; } = null!;
    }
}

