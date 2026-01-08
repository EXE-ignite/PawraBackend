using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionDto>> GetAllAsync();
        Task<PrescriptionDto> GetByIdAsync(Guid id);
        Task<PrescriptionDto> CreateAsync(CreatePrescriptionDto dto);
        Task<PrescriptionDto> UpdateAsync(Guid id, UpdatePrescriptionDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}

