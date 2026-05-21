using PeakyStart.Domain.Models;

namespace PeakyStart.Domain.Interfaces.Repositories
{
    public interface ICurrencyDbRepository : IBaseRepository<Currency>
    {
        Task SaveAllAsync(IEnumerable<Currency> currencies);
        Task<bool> HasDataAsync();
        Task AddAsync(Currency currency);
    }
}
