using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uCondo.Domain.Entities;

namespace uCondo.Data.Mappings
{
    public class BillTypeMapping : IEntityTypeConfiguration<BillType>
    {
        public void Configure(EntityTypeBuilder<BillType> builder)
        {
            builder.ToTable("BillTypes");

            builder.Property(billType => billType.Name).HasMaxLength(255).IsRequired();
        }
    }
}
