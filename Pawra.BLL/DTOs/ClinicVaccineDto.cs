namespace Pawra.BLL.DTOs
{
    public class ClinicVaccineDto
    {
        public Guid Id { get; set; }
        public Guid ClinicId { get; set; }
        public Guid VaccineId { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
