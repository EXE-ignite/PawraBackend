using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IClinicVaccineService
    {
        Task<IEnumerable<ClinicVaccineDto>> GetAllAsync();
        Task<ClinicVaccineDto> GetByIdAsync(Guid id);
        Task<ClinicVaccineDto> CreateAsync(CreateClinicVaccineDto dto);
        Task<ClinicVaccineDto> UpdateAsync(Guid id, UpdateClinicVaccineDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
