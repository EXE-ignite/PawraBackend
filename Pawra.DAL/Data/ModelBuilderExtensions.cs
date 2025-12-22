using Microsoft.EntityFrameworkCore;
using Pawra.DAL.Entities;

namespace Pawra.DAL.Data
{
    public static class ModelBuilderExtensions
    {
        public static void SeedAccountRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountRole>().HasData(
                new AccountRole { Name = "Admin" },
                new AccountRole { Name = "Customer" },
                new AccountRole { Name = "Veterinarian" },
                new AccountRole { Name = "ClinicManager" }
            );
        }
    }
}
