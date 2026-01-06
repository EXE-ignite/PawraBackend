using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public PaymentService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _context.Payments.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> GetByIdAsync(Guid id)
        {
            var payment = await _context.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null) throw new NotFoundException($"Không tìm thấy thanh toán với ID: {id}");
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
        {
            var payment = _mapper.Map<Payment>(dto);
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<PaymentDto> UpdateAsync(Guid id, UpdatePaymentDto dto)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) throw new NotFoundException($"Không tìm thấy thanh toán với ID: {id}");
            _mapper.Map(dto, payment);
            payment.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) throw new NotFoundException($"Không tìm thấy thanh toán với ID: {id}");
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

