using Finsight.Core.Dao.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finsight.Core.Dao.EF.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order)).HasKey(s => s.Id);

            builder.Property(o => o.StartDate).IsRequired();
            builder.Property(o => o.ExpireDate).IsRequired();
            builder.Property(o => o.Status).IsRequired();
            builder.HasOne(o => o.Customer).WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Executor).WithMany()
                .HasForeignKey(o => o.ExecutorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
