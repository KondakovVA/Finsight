using Finsight.Core.Dao.EF.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Finsight.Core.Dao.EF
{
    public class EfContext : DbContext
    {
        public EfContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }
    }
}
