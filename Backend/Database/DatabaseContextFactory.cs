using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database
{
    internal class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuild = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuild.UseNpgsql("Host=localhost;Port=5432;Database=Solf;Username=postgres;Password=111;");

            return new DatabaseContext(optionsBuild.Options);
        }
    }
}
