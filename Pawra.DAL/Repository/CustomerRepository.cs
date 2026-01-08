using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
