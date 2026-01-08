using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.BLL.Services;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.BLL.Service
{
    public class AccountRoleService : BaseService<AccountRole, AccountRoleDto>, IAccountRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountRoleService(IUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork.AccountRoleRepository, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AccountRoleDto>> GetAllAsync()
        {
            var roles = await _unitOfWork.AccountRoleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AccountRoleDto>>(roles);
        }

        public async Task<AccountRoleDto> GetByIdAsync(Guid id)
        {
            var role = await _unitOfWork.AccountRoleRepository.GetByIdAsync(id);

            if (role == null)
            {
                throw new NotFoundException($"Không tìm thấy role với ID: {id}");
            }

            return _mapper.Map<AccountRoleDto>(role);
        }

        public async Task<AccountRoleDto> CreateAsync(CreateAccountRoleDto dto)
        {
            // Kiểm tra trùng tên role
            var exists = await _unitOfWork.AccountRoleRepository.ExistsByNameAsync(dto.Name);

            if (exists)
            {
                throw new Exception($"Role '{dto.Name}' đã tồn tại trong hệ thống");
            }

            var role = _mapper.Map<AccountRole>(dto);
            await _unitOfWork.AccountRoleRepository.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountRoleDto>(role);
        }

        public async Task<AccountRoleDto> UpdateAsync(Guid id, UpdateAccountRoleDto dto)
        {
            var role = await _unitOfWork.AccountRoleRepository.GetByIdAsync(id);

            if (role == null)
            {
                throw new NotFoundException($"Không tìm thấy role với ID: {id}");
            }

            // Kiểm tra trùng tên role (trừ role hiện tại)
            var exists = await _unitOfWork.AccountRoleRepository.ExistsByNameAsync(dto.Name, id);

            if (exists)
            {
                throw new Exception($"Role '{dto.Name}' đã tồn tại trong hệ thống");
            }

            _mapper.Map(dto, role);
            role.SetUpdatedDate();

            await _unitOfWork.AccountRoleRepository.UpdateAsync(role);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountRoleDto>(role);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _unitOfWork.AccountRoleRepository.GetByIdAsync(id);

            if (role == null)
            {
                throw new NotFoundException($"Không tìm thấy role với ID: {id}");
            }

            // Kiểm tra xem có account nào đang sử dụng role này không
            var hasAccounts = await _unitOfWork.AccountRoleRepository.HasAccountsUsingRoleAsync(id);

            if (hasAccounts)
            {
                throw new Exception($"Không thể xóa role '{role.Name}' vì còn account đang sử dụng");
            }

            await _unitOfWork.AccountRoleRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
