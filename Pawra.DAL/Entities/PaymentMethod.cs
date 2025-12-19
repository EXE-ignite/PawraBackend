namespace Pawra.DAL.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

}