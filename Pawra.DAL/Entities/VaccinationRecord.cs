namespace Pawra.DAL.Entities
{
    public class VaccinationRecord : BaseEntity
    {
        public Guid PetId { get; set; }
        public Pet Pet { get; set; } = null!;

        public Guid VaccineId { get; set; }
        public Vaccine Vaccine { get; set; } = null!;

        public Guid ClinicId { get; set; }
        public Clinic Clinic { get; set; } = null!;

        public DateTime VaccinationDate { get; set; }
    }

}