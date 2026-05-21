using Microsoft.EntityFrameworkCore;
using PeakyStart.Domain.Interfaces.Repositories;
using PeakyStart.Domain.Models;
using PeakyStart.Infrastructure.Persistence;

namespace PeakyStart.Infrastructure.Repositories
{
    public class CurrencyDbRepository : ICurrencyDbRepository
    {
        private readonly DatabaseContext _db;

        public CurrencyDbRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _db.Currencies.ToListAsync();
        }

        public async Task SaveAllAsync(IEnumerable<Currency> currencies)
        {
            foreach (var currency in currencies)
            {
                var existing = await _db.Currencies.FindAsync(currency.Id);

                if (existing is null)
                    _db.Currencies.Add(currency);
                else
                {
                    existing.Id = currency.Id;
                    existing.NumCode = currency.NumCode;
                    existing.CharCode = currency.CharCode;
                    existing.Nominal = currency.Nominal;
                    existing.Name = currency.Name;
                    existing.Value = currency.Value;
                    existing.Previous = currency.Previous;
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task<bool> HasDataAsync() => await _db.Currencies.AnyAsync();
    }
}
