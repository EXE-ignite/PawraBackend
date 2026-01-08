using Pawra.DAL.Entities;

namespace Pawra.DAL.Interfaces
{
    public interface IAccountRoleRepository : IRepository<AccountRole>
    {
        /// <summary>
        /// Kiểm tra xem có account nào đang sử dụng role này không
        /// </summary>
        Task<bool> HasAccountsUsingRoleAsync(Guid roleId);

        /// <summary>
        /// Kiểm tra xem role có tồn tại theo tên không (case-insensitive)
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
    }
}
