using AutoHub.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Server.Data
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Step>().ToTable("Steps");
            builder.Entity<Item>().ToTable("Items");
        }

        public DbSet<Step> Steps { get; set; }
        public DbSet<Item> Items { get; set; }
    }

    public static class DbSeeder
    {
        public static void SeedAdminUser(MainDbContext context)
        {
            try
            {
                context.Database.BeginTransaction();
                if (!context.Steps.Any(x => x.Title == "Step 1"))
                {
                    var step = new Step { Title = "Step 1", };
                    context.Entry(step).State = EntityState.Added;
                    context.SaveChanges();
                }
                context.Database.CommitTransaction();
            }
            catch (Exception)
            {
                context.Database.RollbackTransaction();
                throw;
            }
        }
    }
}
