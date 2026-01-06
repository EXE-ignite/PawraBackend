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
            var subscriptions = await _context.SubscriptionAccounts.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<SubscriptionAccountDto>>(subscriptions);
        }

        public async Task<SubscriptionAccountDto> GetByIdAsync(Guid id)
        {
            var subscription = await _context.SubscriptionAccounts.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (subscription == null) throw new NotFoundException($"Không tìm thấy đăng ký với ID: {id}");
            return _mapper.Map<SubscriptionAccountDto>(subscription);
        }

        public async Task<SubscriptionAccountDto> CreateAsync(CreateSubscriptionAccountDto dto)
        {
            var subscription = _mapper.Map<SubscriptionAccount>(dto);
            _context.SubscriptionAccounts.Add(subscription);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubscriptionAccountDto>(subscription);
        }

        public async Task<SubscriptionAccountDto> UpdateAsync(Guid id, UpdateSubscriptionAccountDto dto)
        {
            var subscription = await _context.SubscriptionAccounts.FindAsync(id);
            if (subscription == null) throw new NotFoundException($"Không tìm thấy đăng ký với ID: {id}");
            _mapper.Map(dto, subscription);
            subscription.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<SubscriptionAccountDto>(subscription);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var subscription = await _context.SubscriptionAccounts.FindAsync(id);
            if (subscription == null) throw new NotFoundException($"Không tìm thấy đăng ký với ID: {id}");
            _context.SubscriptionAccounts.Remove(subscription);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

