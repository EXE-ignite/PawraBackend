using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class VaccineRepository : BaseRepository<Vaccine>, IVaccineRepository
    {
        public VaccineRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
