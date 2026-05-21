using PeakyStart.Domain.Interfaces.Repositories;
using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;
using PeakyStart.Infrastructure.Repositories;

namespace PeakyStart.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyApiRepository _httpRepository;
        private readonly ICurrencyDbRepository _localRepository;

        public CurrencyService(ICurrencyApiRepository httpRepository, CurrencyDbRepository localRepository)
        {
            _httpRepository = httpRepository;
            _localRepository = localRepository;
        }
        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            try
            {
                var currencies = await _httpRepository.GetAllAsync();
                await _localRepository.SaveAllAsync(currencies);
            }
            catch (HttpRequestException)
            {
                if (!await _localRepository.HasDataAsync())
                    throw;
            }
            return await _localRepository.GetAllAsync();
        }

        public async Task AddAsync(Currency currency)
        {
            await _localRepository.AddAsync(currency);
        }

        public async Task RefreshAsync()
        {
            var currencies = await _httpRepository.GetAllAsync();
            await _localRepository.SaveAllAsync(currencies);
        }
    }
}
