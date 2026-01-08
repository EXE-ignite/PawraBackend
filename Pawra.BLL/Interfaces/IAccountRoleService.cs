using Pawra.BLL.DTOs;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Interfaces
{
    public interface IAccountRoleService : IService<AccountRole, AccountRoleDto>
    {
        Task<IEnumerable<AccountRoleDto>> GetAllAsync();
        Task<AccountRoleDto> GetByIdAsync(Guid id);
        Task<AccountRoleDto> CreateAsync(CreateAccountRoleDto dto);
        Task<AccountRoleDto> UpdateAsync(Guid id, UpdateAccountRoleDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
