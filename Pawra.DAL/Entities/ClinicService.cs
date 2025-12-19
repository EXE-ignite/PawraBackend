namespace Pawra.DAL.Entities
{
    public class ClinicService : BaseEntity
    {
        public Guid ClinicId { get; set; }
        public Clinic Clinic { get; set; } = null!;

        public Guid ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
    }
}