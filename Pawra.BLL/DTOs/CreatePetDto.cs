using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreatePetDto
    {
        [Required(ErrorMessage = "CustomerId là bắt buộc")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Tên thú cưng là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên thú cưng không được vượt quá 100 ký tự")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Loài là bắt buộc")]
        [StringLength(50, ErrorMessage = "Loài không được vượt quá 50 ký tự")]
        public string Species { get; set; } = null!;

        [Required(ErrorMessage = "Giống loài là bắt buộc")]
        [StringLength(100, ErrorMessage = "Giống loài không được vượt quá 100 ký tự")]
        public string Breed { get; set; } = null!;

        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        public DateTime BirthDate { get; set; }
    }
}

