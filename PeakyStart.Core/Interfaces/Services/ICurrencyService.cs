using PeakyStart.Domain.Interfaces.Repositories;
using PeakyStart.Domain.Models;

namespace PeakyStart.Domain.Interfaces.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetAllAsync();
        Task AddAsync(Currency currency);
        Task RefreshAsync();
        Task DeleteAsync(string id);
        void SetRepository(ICurrencyStorageRepository repository);
    }
}
