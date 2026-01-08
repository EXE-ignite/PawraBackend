using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public PaymentMethodService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentMethodDto>> GetAllAsync()
        {
            var paymentMethods = await _context.PaymentMethods
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<PaymentMethodDto>>(paymentMethods);
        }

        public async Task<PaymentMethodDto> GetByIdAsync(Guid id)
        {
            var paymentMethod = await _context.PaymentMethods
                .AsNoTracking()
                .FirstOrDefaultAsync(pm => pm.Id == id);
            if (paymentMethod == null)
                throw new NotFoundException($"Không tìm thấy phương thức thanh toán với ID: {id}");
            return _mapper.Map<PaymentMethodDto>(paymentMethod);
        }

        public async Task<PaymentMethodDto> CreateAsync(CreatePaymentMethodDto dto)
        {
            var paymentMethod = _mapper.Map<PaymentMethod>(dto);
            _context.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentMethodDto>(paymentMethod);
        }

        public async Task<PaymentMethodDto> UpdateAsync(Guid id, UpdatePaymentMethodDto dto)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethod == null)
                throw new NotFoundException($"Không tìm thấy phương thức thanh toán với ID: {id}");
            _mapper.Map(dto, paymentMethod);
            paymentMethod.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentMethodDto>(paymentMethod);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethod == null)
                throw new NotFoundException($"Không tìm thấy phương thức thanh toán với ID: {id}");
            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

