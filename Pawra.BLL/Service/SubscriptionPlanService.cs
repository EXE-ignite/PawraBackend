using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class SubscriptionPlanService : BaseService<SubscriptionPlan, SubscriptionPlanDto>, ISubscriptionPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionPlanService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.SubscriptionPlanRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SubscriptionPlanDto>> GetAllAsync()
        {
            var plans = await _unitOfWork.SubscriptionPlanRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubscriptionPlanDto>>(plans);
        }

        public async Task<SubscriptionPlanDto> GetByIdAsync(Guid id)
        {
            var plan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(id);
            if (plan == null)
                throw new NotFoundException($"Không tìm thấy gói đăng ký với ID: {id}");
            return _mapper.Map<SubscriptionPlanDto>(plan);
        }

        public async Task<SubscriptionPlanDto> CreateAsync(CreateSubscriptionPlanDto dto)
        {
            var plan = _mapper.Map<SubscriptionPlan>(dto);
            await _unitOfWork.SubscriptionPlanRepository.AddAsync(plan);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SubscriptionPlanDto>(plan);
        }

        public async Task<SubscriptionPlanDto> UpdateAsync(Guid id, UpdateSubscriptionPlanDto dto)
        {
            var plan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(id);
            if (plan == null)
                throw new NotFoundException($"Không tìm thấy gói đăng ký với ID: {id}");
            _mapper.Map(dto, plan);
            plan.SetUpdatedDate();
            await _unitOfWork.SubscriptionPlanRepository.UpdateAsync(plan);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SubscriptionPlanDto>(plan);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var plan = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(id);
            if (plan == null)
                throw new NotFoundException($"Không tìm thấy gói đăng ký với ID: {id}");
            await _unitOfWork.SubscriptionPlanRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
