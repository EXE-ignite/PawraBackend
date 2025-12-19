namespace Pawra.DAL.Entities
{
    public class Customer : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public ICollection<Pet> Pets { get; set; } = new List<Pet>();

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

}