using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IVaccineService
    {
        Task<IEnumerable<VaccineDto>> GetAllAsync();
        Task<VaccineDto> GetByIdAsync(Guid id);
        Task<VaccineDto> CreateAsync(CreateVaccineDto dto);
        Task<VaccineDto> UpdateAsync(Guid id, UpdateVaccineDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}

