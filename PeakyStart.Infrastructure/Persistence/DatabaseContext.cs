using Microsoft.EntityFrameworkCore;
using PeakyStart.Domain.Models;
using Windows.Storage;

namespace PeakyStart.Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        private const string DatabaseFileName = "peakystart.db";
        private const string FallbackFolderName = "PeakyStart";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath;

            try
            {
                var localFolder = ApplicationData.Current.LocalFolder.Path;
                dbPath = Path.Combine(localFolder, DatabaseFileName);
            }
            catch
            {
                dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FallbackFolderName, DatabaseFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
            }

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}