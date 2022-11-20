using Microsoft.EntityFrameworkCore;
using uCondo.Data.Mappings;
using uCondo.Domain.Entities;

namespace uCondo.Data.Context
{
    public class UCondoContext : DbContext
    {
        public UCondoContext(DbContextOptions<UCondoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BillMapping());
            builder.ApplyConfiguration(new BillTypeMapping());
        }

        public virtual DbSet<Bill> Bills { get; set; }

        public virtual DbSet<BillType> BillTypes { get; set; }
    }
}
