using Microsoft.EntityFrameworkCore;
using Paybliss.Models;

namespace Paybliss.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(b => b.custormerId)
                .IsRequired(false)//optinal case
                ;
        }
        public DbSet<User> User { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<CableValues> Cables {  get; set; }
        public DbSet<AccountDetails> AccountDetails { get; set; }
    }
}
