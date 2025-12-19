namespace Pawra.DAL.Entities
{
    public class Veterinarian : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public Guid ClinicId { get; set; }
        public Clinic Clinic { get; set; } = null!;

        public string LicenseNumber { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; }
        = new List<Appointment>();
    }

}