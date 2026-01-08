using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class PrescriptionRepository : BaseRepository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
