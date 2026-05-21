using PeakyStart.Domain.Interfaces.Repositories;
using PeakyStart.Domain.Models;
using PeakyStart.Infrastructure.DTO;
using System.Text.Json;

namespace PeakyStart.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private const string URL = "https://www.cbr-xml-daily.ru/daily_json.js";
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly HttpClient _httpClient;

        public CurrencyRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            var json = await _httpClient.GetStringAsync(URL);
            var response = JsonSerializer.Deserialize<CurrenciesResponseDTO>(json, _jsonOptions);

            return response?.Valute?.Values
                .Select(x => new Currency
                {
                    Id = x.Id,
                    NumCode = x.NumCode,
                    CharCode = x.CharCode,
                    Nominal = x.Nominal,
                    Name = x.Name,
                    Value = x.Value,
                    Previous = x.Previous
                })
                .ToList() ?? new List<Currency>();
        }
    }
}
