namespace Pawra.DAL.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public DateTime CreatedDate { get; protected set; }
        public DateTime? UpdatedDate { get; protected set; }
        public bool IsDeleted { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }

        public void SetUpdatedDate()
        {
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
