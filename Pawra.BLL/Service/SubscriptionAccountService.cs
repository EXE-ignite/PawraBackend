using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class SubscriptionAccountService : BaseService<SubscriptionAccount, SubscriptionAccountDto>, ISubscriptionAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionAccountService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.SubscriptionAccountRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SubscriptionAccountDto>> GetAllAsync()
        {
            var subscriptionAccounts = await _unitOfWork.SubscriptionAccountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubscriptionAccountDto>>(subscriptionAccounts);
        }

        public async Task<SubscriptionAccountDto> GetByIdAsync(Guid id)
        {
            var subscriptionAccount = await _unitOfWork.SubscriptionAccountRepository.GetByIdAsync(id);
            if (subscriptionAccount == null)
                throw new NotFoundException($"Không tìm thấy đăng ký gói với ID: {id}");
            return _mapper.Map<SubscriptionAccountDto>(subscriptionAccount);
        }

        public async Task<SubscriptionAccountDto> CreateAsync(CreateSubscriptionAccountDto dto)
        {
            var subscriptionAccount = _mapper.Map<SubscriptionAccount>(dto);
            await _unitOfWork.SubscriptionAccountRepository.AddAsync(subscriptionAccount);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SubscriptionAccountDto>(subscriptionAccount);
        }

        public async Task<SubscriptionAccountDto> UpdateAsync(Guid id, UpdateSubscriptionAccountDto dto)
        {
            var subscriptionAccount = await _unitOfWork.SubscriptionAccountRepository.GetByIdAsync(id);
            if (subscriptionAccount == null)
                throw new NotFoundException($"Không tìm thấy đăng ký gói với ID: {id}");
            _mapper.Map(dto, subscriptionAccount);
            subscriptionAccount.SetUpdatedDate();
            await _unitOfWork.SubscriptionAccountRepository.UpdateAsync(subscriptionAccount);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SubscriptionAccountDto>(subscriptionAccount);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var subscriptionAccount = await _unitOfWork.SubscriptionAccountRepository.GetByIdAsync(id);
            if (subscriptionAccount == null)
                throw new NotFoundException($"Không tìm thấy đăng ký gói với ID: {id}");
            await _unitOfWork.SubscriptionAccountRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
