using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface ISubscriptionAccountService
    {
        Task<IEnumerable<SubscriptionAccountDto>> GetAllAsync();
        Task<SubscriptionAccountDto> GetByIdAsync(Guid id);
        Task<SubscriptionAccountDto> CreateAsync(CreateSubscriptionAccountDto dto);
        Task<SubscriptionAccountDto> UpdateAsync(Guid id, UpdateSubscriptionAccountDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}

