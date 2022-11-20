using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uCondo.Domain.Entities;

namespace uCondo.Data.Mappings
{
    public class BillMapping : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable("Bills");

            builder.Property(bill => bill.Name).HasMaxLength(255).IsRequired();

            builder.Property(bill => bill.Code).HasMaxLength(55).IsRequired();

            builder.Property(bill => bill.AcceptReleases).IsRequired();

            builder.HasIndex(index => index.Code);

            builder.HasOne(bill => bill.Type)
                .WithMany(type => type.Bills)
                .HasForeignKey(f => f.TypeId)
                .HasConstraintName("FK__Bill__BillType");
        }
    }
}
