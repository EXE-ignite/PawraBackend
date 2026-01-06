namespace Pawra.BLL.DTOs
{
    public class PrescriptionDto
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public string Notes { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

