using PeakyStart.Domain.Interfaces.Repositories;
using PeakyStart.Domain.Models;
using System.Text.Json;

namespace PeakyStart.Infrastructure.Repositories
{
    public class CurrencyJsonRepository : ICurrencyJsonRepository
    {
        private const string FileName = "currencies.json";
        private const string FolderName = "PeakyStart";

        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };

        private string GetFilePath()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FolderName);

            Directory.CreateDirectory(folder);

            return Path.Combine(folder, FileName);
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            var path = GetFilePath();

            if (!File.Exists(path))
                return new List<Currency>();

            var json = await File.ReadAllTextAsync(path);

            return JsonSerializer.Deserialize<List<Currency>>(json, _options) ?? new List<Currency>();
        }

        public async Task SaveAllAsync(IEnumerable<Currency> currencies)
        {
            var json = JsonSerializer.Serialize(currencies, _options);

            await File.WriteAllTextAsync(GetFilePath(), json);
        }

        public async Task AddAsync(Currency currency)
        {
            var currencies = (await GetAllAsync()).ToList();

            currency.IsUserDefined = true;

            currencies.Add(currency);

            await SaveAllAsync(currencies);
        }

        public async Task DeleteAsync(string id)
        {
            var currencies = (await GetAllAsync()).ToList();

            var entity = currencies.FirstOrDefault(x => x.Id == id);

            if (entity is not null)
                currencies.Remove(entity);

            await SaveAllAsync(currencies);
        }

        public async Task<bool> HasDataAsync()
        {
            return (await GetAllAsync()).Any();
        }
    }
}
