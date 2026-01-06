using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IClinicService
    {
        Task<IEnumerable<ClinicDto>> GetAllAsync();
        Task<ClinicDto> GetByIdAsync(Guid id);
        Task<ClinicDto> CreateAsync(CreateClinicDto dto);
        Task<ClinicDto> UpdateAsync(Guid id, UpdateClinicDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}

