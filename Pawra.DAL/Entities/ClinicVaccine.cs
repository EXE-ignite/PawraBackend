using System.Reflection.PortableExecutable;

namespace Pawra.DAL.Entities
{
    public class ClinicVaccine : BaseEntity
    {
        public Guid ClinicId { get; set; }
        public Clinic Clinic { get; set; } = null!;

        public Guid VaccineId { get; set; }
        public Vaccine Vaccine { get; set; } = null!;

        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
    }
}