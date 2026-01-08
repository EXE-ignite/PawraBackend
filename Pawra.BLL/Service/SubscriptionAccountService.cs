using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class SubscriptionAccountService : ISubscriptionAccountService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public SubscriptionAccountService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubscriptionAccountDto>> GetAllAsync()
        {
            var subscriptionAccounts = await _context.SubscriptionAccounts
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<SubscriptionAccountDto>>(subscriptionAccounts);
        }

        public async Task<SubscriptionAccountDto> GetByIdAsync(Guid id)
        {
            var subscriptionAccount = await _context.SubscriptionAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(sa => sa.Id == id);
            if (subscriptionAccount == null)
                throw new NotFoundException($"Không tìm thấy đăng ký gói với ID: {id}");
            return _mapper.Map<SubscriptionAccountDto>(subscriptionAccount);
        }

        public async Task<SubscriptionAccountDto> CreateAsync(CreateSubscriptionAccountDto dto)
        {
            var subscriptionAccount = _mapper.Map<SubscriptionAccount>(dto);
            _context.SubscriptionAccounts.Add(subscriptionAccount);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubscriptionAccountDto>(subscriptionAccount);
        }

        public async Task<SubscriptionAccountDto> UpdateAsync(Guid id, UpdateSubscriptionAccountDto dto)
        {
            var subscriptionAccount = await _context.SubscriptionAccounts.FindAsync(id);
            if (subscriptionAccount == null)
                throw new NotFoundException($"Không tìm thấy đăng ký gói với ID: {id}");
            _mapper.Map(dto, subscriptionAccount);
            subscriptionAccount.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<SubscriptionAccountDto>(subscriptionAccount);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var subscriptionAccount = await _context.SubscriptionAccounts.FindAsync(id);
            if (subscriptionAccount == null)
                throw new NotFoundException($"Không tìm thấy đăng ký gói với ID: {id}");
            _context.SubscriptionAccounts.Remove(subscriptionAccount);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

