using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Service
{
    public class AccountRoleService : IAccountRoleService
    {
        private readonly PawraDBContext _context;
        private readonly IMapper _mapper;

        public AccountRoleService(PawraDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountRoleDto>> GetAllAsync()
        {
            var roles = await _context.AccountRoles
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<AccountRoleDto>>(roles);
        }

        public async Task<AccountRoleDto> GetByIdAsync(Guid id)
        {
            var role = await _context.AccountRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                throw new NotFoundException($"Không tìm thấy role với ID: {id}");
            }

            return _mapper.Map<AccountRoleDto>(role);
        }

        public async Task<AccountRoleDto> CreateAsync(CreateAccountRoleDto dto)
        {
            // Kiểm tra trùng tên role
            var existingRole = await _context.AccountRoles
                .FirstOrDefaultAsync(r => r.Name.ToLower() == dto.Name.ToLower());

            if (existingRole != null)
            {
                throw new Exception($"Role '{dto.Name}' đã tồn tại trong hệ thống");
            }

            var role = _mapper.Map<AccountRole>(dto);

            _context.AccountRoles.Add(role);
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountRoleDto>(role);
        }

        public async Task<AccountRoleDto> UpdateAsync(Guid id, UpdateAccountRoleDto dto)
        {
            var role = await _context.AccountRoles.FindAsync(id);

            if (role == null)
            {
                throw new NotFoundException($"Không tìm thấy role với ID: {id}");
            }

            // Kiểm tra trùng tên role (trừ role hiện tại)
            var existingRole = await _context.AccountRoles
                .FirstOrDefaultAsync(r => r.Name.ToLower() == dto.Name.ToLower() && r.Id != id);

            if (existingRole != null)
            {
                throw new Exception($"Role '{dto.Name}' đã tồn tại trong hệ thống");
            }

            _mapper.Map(dto, role);
            role.SetUpdatedDate();

            await _context.SaveChangesAsync();

            return _mapper.Map<AccountRoleDto>(role);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _context.AccountRoles.FindAsync(id);

            if (role == null)
            {
                throw new NotFoundException($"Không tìm thấy role với ID: {id}");
            }

            // Kiểm tra xem có account nào đang sử dụng role này không
            var hasAccounts = await _context.Accounts.AnyAsync(a => a.RoleId == id);

            if (hasAccounts)
            {
                throw new Exception($"Không thể xóa role '{role.Name}' vì còn account đang sử dụng");
            }

            _context.AccountRoles.Remove(role);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
