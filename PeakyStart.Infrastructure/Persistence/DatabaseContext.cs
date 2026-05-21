using Microsoft.EntityFrameworkCore;
using PeakyStart.Domain.Models;
using Windows.Storage;

namespace PeakyStart.Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath;

            try
            {
                var localFolder = ApplicationData.Current.LocalFolder.Path;
                dbPath = Path.Combine(localFolder, "peakystart.db");
            }
            catch
            {
                dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "PeakyStart",
                    "peakystart.db");

                Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
            }

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}