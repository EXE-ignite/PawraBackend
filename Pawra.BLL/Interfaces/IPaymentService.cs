using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<PaymentDto> GetByIdAsync(Guid id);
        Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
        Task<PaymentDto> UpdateAsync(Guid id, UpdatePaymentDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
