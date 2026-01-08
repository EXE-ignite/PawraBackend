namespace Pawra.BLL.DTOs
{
    public class VaccinationRecordDto
    {
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public Guid VaccineId { get; set; }
        public Guid ClinicId { get; set; }
        public DateTime VaccinationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
