using Microsoft.EntityFrameworkCore;
using Pawra.DAL.Entities;
using System.Reflection;

namespace Pawra.DAL.Data
{
    public static class ModelBuilderExtensions
    {
        public static void SeedAccountRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountRole>().HasData(
                new
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Admin",
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedDate = (DateTime?)null,
                    IsDeleted = false
                },
                new
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Customer",
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedDate = (DateTime?)null,
                    IsDeleted = false
                },
                new
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Veterinarian",
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedDate = (DateTime?)null,
                    IsDeleted = false
                },
                new
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "ClinicManager",
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedDate = (DateTime?)null,
                    IsDeleted = false
                }
            );
        }

        /// <summary>
        /// Seeding tất cả dữ liệu (roles + admin account)
        /// </summary>
        public static void EnsureSeedData(this PawraDBContext context)
        {
            // Seed AccountRoles
            var roles = new List<(Guid Id, string Name)>
            {
                (Guid.Parse("11111111-1111-1111-1111-111111111111"), "Admin"),
                (Guid.Parse("22222222-2222-2222-2222-222222222222"), "Customer"),
                (Guid.Parse("33333333-3333-3333-3333-333333333333"), "Veterinarian"),
                (Guid.Parse("44444444-4444-4444-4444-444444444444"), "ClinicManager"),
            };

            foreach (var (id, name) in roles)
            {
                if (!context.AccountRoles.Any(r => r.Id == id))
                {
                    var role = new AccountRole { Name = name };
                    typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id))?.SetValue(role, id);
                    context.AccountRoles.Add(role);
                }
            }

            // Seed Admin Account
            var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var adminAccountId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            if (!context.Accounts.Any(a => a.Id == adminAccountId))
            {
                var adminAccount = new Account
                {
                    Email = "admin@pawra.com",
                    FullName = "Admin",
                    PasswordHash = "hashedpassword123",
                    RoleId = adminRoleId
                };

                typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id))?.SetValue(adminAccount, adminAccountId);
                context.Accounts.Add(adminAccount);
            }

            // Save tất cả cùng 1 lượt
            context.SaveChanges();
        }
    }
}
