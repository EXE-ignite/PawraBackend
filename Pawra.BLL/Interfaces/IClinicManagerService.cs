using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IClinicManagerService
    {
        Task<IEnumerable<ClinicManagerDto>> GetAllAsync();
        Task<ClinicManagerDto> GetByIdAsync(Guid id);
        Task<ClinicManagerDto> CreateAsync(CreateClinicManagerDto dto);
        Task<ClinicManagerDto> UpdateAsync(Guid id, UpdateClinicManagerDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}

