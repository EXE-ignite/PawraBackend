namespace Pawra.BLL.DTOs
{
    public class VeterinarianDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid ClinicId { get; set; }
        public string LicenseNumber { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

