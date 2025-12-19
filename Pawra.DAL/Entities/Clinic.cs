namespace Pawra.DAL.Entities
{
    public class Clinic : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public Guid ClinicManagerId { get; set; }
        public ClinicManager ClinicManager { get; set; } = null!;

        public ICollection<Veterinarian> Veterinarians { get; set; } = new List<Veterinarian>();

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<ClinicVaccine> ClinicVaccines { get; set; } = new List<ClinicVaccine>();

        public ICollection<ClinicService> ClinicServices { get; set; } = new List<ClinicService>();

        public ICollection<VaccinationRecord> VaccinationRecords { get; set; } = new List<VaccinationRecord>();
    }

}