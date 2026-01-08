namespace Pawra.BLL.DTOs
{
    public class ClinicServiceDto
    {
        public Guid Id { get; set; }
        public Guid ClinicId { get; set; }
        public Guid ServiceId { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
