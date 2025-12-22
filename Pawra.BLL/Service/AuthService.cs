using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pawra.BLL.DTOs;
using Pawra.BLL.Exceptions;
using Pawra.BLL.Interfaces;
using Pawra.DAL;
using Pawra.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pawra.BLL.Service
{
    public class AuthService : IAuthService
    {
        private readonly PawraDBContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(PawraDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            // Tìm account theo email
            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(a => a.Email == request.Email);

            if (account == null)
            {
                throw new NotFoundException("Email hoặc password không đúng");
            }

            // Verify password
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash);
            if (!isValidPassword)
            {
                throw new NotFoundException("Email hoặc password không đúng");
            }

            // Tạo JWT token
            var token = GenerateJwtToken(account);

            return new LoginResponseDto
            {
                Token = token,
                Email = account.Email,
                FullName = account.FullName,
                Role = account.Role.Name,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Kiểm tra email đã tồn tại chưa
            var existingAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == request.Email);

            if (existingAccount != null)
            {
                throw new Exception("Email đã được sử dụng");
            }

            // Tự động gán role customer
            var role = await _context.AccountRoles
                .FirstOrDefaultAsync(r => r.Name.ToLower() == "customer");

            if (role == null)
            {
                throw new NotFoundException("Role 'customer' không tồn tại trong hệ thống");
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Tạo account mới
            var newAccount = new Account
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                FullName = request.FullName,
                RoleId = role.Id
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            // Load role để generate token
            await _context.Entry(newAccount).Reference(a => a.Role).LoadAsync();

            // Tạo JWT token
            var token = GenerateJwtToken(newAccount);

            return new LoginResponseDto
            {
                Token = token,
                Email = newAccount.Email,
                FullName = newAccount.FullName,
                Role = newAccount.Role.Name,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        private string GenerateJwtToken(Account account)
        {
            var jwtKey = _configuration["JwtSettings:Key"] 
                ?? throw new InvalidOperationException("JWT Key không được cấu hình");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Name, account.FullName),
                new Claim(ClaimTypes.Role, account.Role.Name)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
