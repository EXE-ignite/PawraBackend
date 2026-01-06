using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IPetService
    {
        Task<IEnumerable<PetDto>> GetAllAsync();
        Task<PetDto> GetByIdAsync(Guid id);
        Task<PetDto> CreateAsync(CreatePetDto dto);
        Task<PetDto> UpdateAsync(Guid id, UpdatePetDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}

