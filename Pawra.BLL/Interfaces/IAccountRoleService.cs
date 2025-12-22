using Pawra.BLL.DTOs;

namespace Pawra.BLL.Interfaces
{
    public interface IAccountRoleService
    {
        Task<IEnumerable<AccountRoleDto>> GetAllAsync();
        Task<AccountRoleDto> GetByIdAsync(Guid id);
        Task<AccountRoleDto> CreateAsync(CreateAccountRoleDto dto);
        Task<AccountRoleDto> UpdateAsync(Guid id, UpdateAccountRoleDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
