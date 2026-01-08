using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class VeterinarianRepository : BaseRepository<Veterinarian>, IVeterinarianRepository
    {
        public VeterinarianRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
