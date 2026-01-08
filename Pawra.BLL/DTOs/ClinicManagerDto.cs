namespace Pawra.BLL.DTOs
{
    public class ClinicManagerDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
