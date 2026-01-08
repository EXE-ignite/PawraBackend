using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public SubscriptionPlanService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubscriptionPlanDto>> GetAllAsync()
        {
            var plans = await _context.SubscriptionPlans
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<SubscriptionPlanDto>>(plans);
        }

        public async Task<SubscriptionPlanDto> GetByIdAsync(Guid id)
        {
            var plan = await _context.SubscriptionPlans
                .AsNoTracking()
                .FirstOrDefaultAsync(sp => sp.Id == id);
            if (plan == null)
                throw new NotFoundException($"Không tìm thấy gói đăng ký với ID: {id}");
            return _mapper.Map<SubscriptionPlanDto>(plan);
        }

        public async Task<SubscriptionPlanDto> CreateAsync(CreateSubscriptionPlanDto dto)
        {
            var plan = _mapper.Map<SubscriptionPlan>(dto);
            _context.SubscriptionPlans.Add(plan);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubscriptionPlanDto>(plan);
        }

        public async Task<SubscriptionPlanDto> UpdateAsync(Guid id, UpdateSubscriptionPlanDto dto)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(id);
            if (plan == null)
                throw new NotFoundException($"Không tìm thấy gói đăng ký với ID: {id}");
            _mapper.Map(dto, plan);
            plan.SetUpdatedDate();
            await _context.SaveChangesAsync();
            return _mapper.Map<SubscriptionPlanDto>(plan);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var plan = await _context.SubscriptionPlans.FindAsync(id);
            if (plan == null)
                throw new NotFoundException($"Không tìm thấy gói đăng ký với ID: {id}");
            _context.SubscriptionPlans.Remove(plan);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

