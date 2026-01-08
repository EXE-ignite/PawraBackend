using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IClinicServiceService
    {
        Task<IEnumerable<ClinicServiceDto>> GetAllAsync();
        Task<ClinicServiceDto> GetByIdAsync(Guid id);
        Task<ClinicServiceDto> CreateAsync(CreateClinicServiceDto dto);
        Task<ClinicServiceDto> UpdateAsync(Guid id, UpdateClinicServiceDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}

