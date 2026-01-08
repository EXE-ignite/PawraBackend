using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class PaymentMethodService : BaseService<PaymentMethod, PaymentMethodDto>, IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentMethodService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.PaymentMethodRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PaymentMethodDto>> GetAllAsync()
        {
            var paymentMethods = await _unitOfWork.PaymentMethodRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentMethodDto>>(paymentMethods);
        }

        public async Task<PaymentMethodDto> GetByIdAsync(Guid id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null)
                throw new NotFoundException($"Không tìm thấy phương thức thanh toán với ID: {id}");
            return _mapper.Map<PaymentMethodDto>(paymentMethod);
        }

        public async Task<PaymentMethodDto> CreateAsync(CreatePaymentMethodDto dto)
        {
            var paymentMethod = _mapper.Map<PaymentMethod>(dto);
            await _unitOfWork.PaymentMethodRepository.AddAsync(paymentMethod);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PaymentMethodDto>(paymentMethod);
        }

        public async Task<PaymentMethodDto> UpdateAsync(Guid id, UpdatePaymentMethodDto dto)
        {
            var paymentMethod = await _unitOfWork.PaymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null)
                throw new NotFoundException($"Không tìm thấy phương thức thanh toán với ID: {id}");
            _mapper.Map(dto, paymentMethod);
            paymentMethod.SetUpdatedDate();
            await _unitOfWork.PaymentMethodRepository.UpdateAsync(paymentMethod);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PaymentMethodDto>(paymentMethod);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null)
                throw new NotFoundException($"Không tìm thấy phương thức thanh toán với ID: {id}");
            await _unitOfWork.PaymentMethodRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
