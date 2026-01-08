using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDto>> GetAllAsync();
        Task<PaymentMethodDto> GetByIdAsync(Guid id);
        Task<PaymentMethodDto> CreateAsync(CreatePaymentMethodDto dto);
        Task<PaymentMethodDto> UpdateAsync(Guid id, UpdatePaymentMethodDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
