namespace Pawra.DAL.Entities
{
    public class ClinicManager : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public ICollection<Clinic> Clinics { get; set; }
            = new List<Clinic>();
    }
}