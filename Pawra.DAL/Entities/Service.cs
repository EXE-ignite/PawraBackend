namespace Pawra.DAL.Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public ICollection<ClinicService> ClinicServices { get; set; } = new List<ClinicService>();
        public ICollection<Prescription> prescriptions { get; set; } = new List<Prescription>();
    }

}