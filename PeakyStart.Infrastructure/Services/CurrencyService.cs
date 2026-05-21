using PeakyStart.Domain.Interfaces.Repositories;
using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;

namespace PeakyStart.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository) 
        {
            _currencyRepository = currencyRepository;
        }
        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _currencyRepository.GetAllAsync();
        }
    }
}
