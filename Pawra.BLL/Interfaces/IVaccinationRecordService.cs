using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IVaccinationRecordService
    {
        Task<IEnumerable<VaccinationRecordDto>> GetAllAsync();
        Task<VaccinationRecordDto> GetByIdAsync(Guid id);
        Task<VaccinationRecordDto> CreateAsync(CreateVaccinationRecordDto dto);
        Task<VaccinationRecordDto> UpdateAsync(Guid id, UpdateVaccinationRecordDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
