using Microsoft.EntityFrameworkCore;
using Pawra.DAL.Entities;

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
    }
}
