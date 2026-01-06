namespace Pawra.BLL.DTOs
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VeterinarianId { get; set; }
        public Guid ClinicId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

