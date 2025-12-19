namespace Pawra.DAL.Entities
{
    public class Pet : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Species { get; set; } = null!;
        public string Breed { get; set; } = null!; //giống loài
        public DateTime BirthDate { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<VaccinationRecord> VaccinationRecords { get; set; } = new List<VaccinationRecord>();
    }

}