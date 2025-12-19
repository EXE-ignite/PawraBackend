namespace Pawra.DAL.Entities
{
    public class Prescription : BaseEntity
    {
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;

        public string Notes { get; set; } = null!;
    }
}