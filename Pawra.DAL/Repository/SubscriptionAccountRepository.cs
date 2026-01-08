using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class SubscriptionAccountRepository : BaseRepository<SubscriptionAccount>, ISubscriptionAccountRepository
    {
        public SubscriptionAccountRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
