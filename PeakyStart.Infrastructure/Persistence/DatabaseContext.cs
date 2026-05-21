using Microsoft.EntityFrameworkCore;
using PeakyStart.Domain.Models;

namespace PeakyStart.Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "peakystart.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
