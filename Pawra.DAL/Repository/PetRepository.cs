using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class PetRepository : BaseRepository<Pet>, IPetRepository
    {
        public PetRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
