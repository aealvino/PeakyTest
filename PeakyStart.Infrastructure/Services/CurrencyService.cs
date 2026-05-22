using PeakyStart.Domain.Interfaces.Repositories;
using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;

namespace PeakyStart.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyApiRepository _httpRepository;
        private ICurrencyStorageRepository _storageRepository;

        public CurrencyService(ICurrencyApiRepository httpRepository, ICurrencyStorageRepository storageRepository)
        {
            _httpRepository = httpRepository;
            _storageRepository = storageRepository;
        }
        public void SetRepository(ICurrencyStorageRepository repository)
        {
            _storageRepository = repository;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            if (!await _storageRepository.HasDataAsync())
            {
                var currencies = await _httpRepository.GetAllAsync();
                await _storageRepository.SaveAllAsync(currencies);
            }

            return await _storageRepository.GetAllAsync();
        }

        public async Task AddAsync(Currency currency)
        {
            await _storageRepository.AddAsync(currency);
        }

        public async Task RefreshAsync()
        {
            var currencies = await _httpRepository.GetAllAsync();
            await _storageRepository.SaveAllAsync(currencies);
        }

        public async Task DeleteAsync(string id)
        {
            await _storageRepository.DeleteAsync(id);
        }
    }
}