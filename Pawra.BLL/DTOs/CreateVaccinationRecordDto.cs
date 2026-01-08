using System.ComponentModel.DataAnnotations;

namespace Pawra.BLL.DTOs
{
    public class CreateVaccinationRecordDto
    {
        [Required(ErrorMessage = "PetId là bắt buộc")]
        public Guid PetId { get; set; }

        [Required(ErrorMessage = "VaccineId là bắt buộc")]
        public Guid VaccineId { get; set; }

        [Required(ErrorMessage = "ClinicId là bắt buộc")]
        public Guid ClinicId { get; set; }

        [Required(ErrorMessage = "Ngày tiêm chủng là bắt buộc")]
        public DateTime VaccinationDate { get; set; }
    }
}

