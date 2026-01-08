using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class VaccinationRecordRepository : BaseRepository<VaccinationRecord>, IVaccinationRecordRepository
    {
        public VaccinationRecordRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }
    }
}
