namespace Pawra.DAL.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid PetId { get; set; }
        public Pet Pet { get; set; } = null!;

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public Guid VeterinarianId { get; set; }
        public Veterinarian Veterinarian { get; set; } = null!;

        public Guid ClinicId { get; set; }
        public Clinic Clinic { get; set; } = null!;

        public DateTime AppointmentTime { get; set; }
        public string Status { get; set; } = null!;

        public ICollection<Prescription> Prescription { get; set; } = new List<Prescription>();
    }

}