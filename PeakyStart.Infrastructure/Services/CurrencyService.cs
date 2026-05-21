using PeakyStart.Domain.Interfaces.Repositories;
using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;
using PeakyStart.Infrastructure.Repositories;

namespace PeakyStart.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _httpRepository;
        private readonly CurrencyLocalRepository _localRepository;

        public CurrencyService(ICurrencyRepository httpRepository, CurrencyLocalRepository localRepository)
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
                return currencies;
            }
            catch (HttpRequestException)
            {
                if (await _localRepository.HasDataAsync())
                    return await _localRepository.GetAllAsync();
                throw;
            }
        }
    }
}
