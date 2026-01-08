using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<IEnumerable<SubscriptionPlanDto>> GetAllAsync();
        Task<SubscriptionPlanDto> GetByIdAsync(Guid id);
        Task<SubscriptionPlanDto> CreateAsync(CreateSubscriptionPlanDto dto);
        Task<SubscriptionPlanDto> UpdateAsync(Guid id, UpdateSubscriptionPlanDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
