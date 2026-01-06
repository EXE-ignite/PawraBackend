namespace Pawra.BLL.DTOs
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Phone { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

