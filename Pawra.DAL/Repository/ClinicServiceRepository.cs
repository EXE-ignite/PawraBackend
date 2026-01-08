using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class ClinicServiceRepository : BaseRepository<ClinicService>, IClinicServiceRepository
    {
        public ClinicServiceRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
