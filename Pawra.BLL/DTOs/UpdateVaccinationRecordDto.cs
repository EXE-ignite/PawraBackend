using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class UpdateVaccinationRecordDto
    {
        [Required(ErrorMessage = "Ngày tiêm chủng là bắt buộc")]
        public DateTime VaccinationDate { get; set; }
    }
}

