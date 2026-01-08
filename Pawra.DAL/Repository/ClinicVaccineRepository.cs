using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class ClinicVaccineRepository : BaseRepository<ClinicVaccine>, IClinicVaccineRepository
    {
        public ClinicVaccineRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
