using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class ClinicManagerRepository : BaseRepository<ClinicManager>, IClinicManagerRepository
    {
        public ClinicManagerRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
