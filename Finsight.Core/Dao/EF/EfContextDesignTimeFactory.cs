using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Finsight.Core.Dao.EF
{
    public class EfContextDesignTimeFactory : IDesignTimeDbContextFactory<EfContext>
    {
        public EfContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? configuration["DefaultConnection"]
                ?? string.Empty;

            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(connectionString);
            return new EfContext(builder.Options);
        }
    }
}
