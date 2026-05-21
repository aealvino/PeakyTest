using PeakyStart.Domain.Models;

namespace PeakyStart.Domain.Interfaces.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetAllAsync();

        Task AddAsync(Currency currency);
    }
}
