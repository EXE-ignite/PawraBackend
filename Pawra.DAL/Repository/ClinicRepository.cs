using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class ClinicRepository : BaseRepository<Clinic>, IClinicRepository
    {
        public ClinicRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
