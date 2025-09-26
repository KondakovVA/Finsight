using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Finsight.Core.Dao.Model;

namespace Finsight.Core.Dao.EF.Configuration
{
    internal class CustomerConfiguration: IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(nameof(Customer));
            builder.HasKey(s => s.Id);
            builder.HasIndex(s => s.CompanyName).IsUnique();
        }
    }
}
