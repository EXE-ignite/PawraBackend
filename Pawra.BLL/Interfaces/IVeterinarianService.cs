using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IVeterinarianService
    {
        Task<IEnumerable<VeterinarianDto>> GetAllAsync();
        Task<VeterinarianDto> GetByIdAsync(Guid id);
        Task<VeterinarianDto> CreateAsync(CreateVeterinarianDto dto);
        Task<VeterinarianDto> UpdateAsync(Guid id, UpdateVeterinarianDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}

