using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace ProductApi.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

        //public DbSet<Commands> Commands { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //        .Entity<Platform>()
        //        .HasMany(x => x.Commands)
        //        .WithOne(x => x.Platform!)
        //        .HasForeignKey(x => x.PlatformId)
        //        ;
        //    modelBuilder
        //        .Entity<Commands>()
        //        .HasOne(x => x.Platform)
        //        .WithMany(x => x.Commands)
        //        .HasForeignKey(x => x.PlatformId)
        //        ;


//        }

    }
}