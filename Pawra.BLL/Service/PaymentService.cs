using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class PaymentService : BaseService<Payment, PaymentDto>, IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.PaymentRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> GetByIdAsync(Guid id)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);
            if (payment == null)
                throw new NotFoundException($"Không tìm thấy thanh toán với ID: {id}");
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
        {
            var payment = _mapper.Map<Payment>(dto);
            await _unitOfWork.PaymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<PaymentDto> UpdateAsync(Guid id, UpdatePaymentDto dto)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);
            if (payment == null)
                throw new NotFoundException($"Không tìm thấy thanh toán với ID: {id}");
            _mapper.Map(dto, payment);
            payment.SetUpdatedDate();
            await _unitOfWork.PaymentRepository.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(id);
            if (payment == null)
                throw new NotFoundException($"Không tìm thấy thanh toán với ID: {id}");
            await _unitOfWork.PaymentRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
