namespace Pawra.DAL.Entities
{
    public class Vaccine : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Manufacturer { get; set; } = null!;

        public ICollection<ClinicVaccine> ClinicVaccines { get; set; }
            = new List<ClinicVaccine>();

        public ICollection<VaccinationRecord> VaccinationRecords { get; set; }
            = new List<VaccinationRecord>();
    }

}