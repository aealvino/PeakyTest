using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;
using PeakyStart.Infrastructure.Persistence;

namespace PeakyStart.Infrastructure.Services
{
    public class SettingsService : ISettingsService
    {
        private const string LastSessionKey = "LastSessionDateTime";
        private readonly DatabaseContext _db;

        public SettingsService(DatabaseContext db)
        {
            _db = db;
        }

        public void SaveLastSession()
        {
            var existing = _db.AppSettings.FirstOrDefault(s => s.Key == LastSessionKey);

            if (existing is null)
            {
                _db.AppSettings.Add(new AppSetting
                {
                    Key = LastSessionKey,
                    Value = DateTimeOffset.Now.ToString("O")
                });
            }
            else
                existing.Value = DateTimeOffset.Now.ToString("O");
            

            _db.SaveChanges();
        }

        public DateTimeOffset? LoadLastSession()
        {
            var setting = _db.AppSettings.FirstOrDefault(s => s.Key == LastSessionKey);

            if (setting is not null && DateTimeOffset.TryParse(setting.Value, out var result))
            {
                return result;
            }

            return null;
        }
    }
}