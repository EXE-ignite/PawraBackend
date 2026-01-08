using Microsoft.EntityFrameworkCore;
using Pawra.DAL.Entities;
using Pawra.DAL.Interfaces;

namespace Pawra.DAL.Repository
{
    public class AccountRoleRepository : BaseRepository<AccountRole>, IAccountRoleRepository
    {
        public AccountRoleRepository(PawraDBContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Kiểm tra xem có account nào đang sử dụng role này không
        /// </summary>
        public async Task<bool> HasAccountsUsingRoleAsync(Guid roleId)
        {
            return await dbContext.Accounts.AnyAsync(a => a.RoleId == roleId);
        }

        /// <summary>
        /// Kiểm tra xem role có tồn tại theo tên không (case-insensitive)
        /// </summary>
        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
        {
            var query = dbContext.AccountRoles.AsNoTracking()
                .Where(r => r.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(r => r.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
