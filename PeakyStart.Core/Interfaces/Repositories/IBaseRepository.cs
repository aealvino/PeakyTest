using PeakyStart.Domain.Models;

namespace PeakyStart.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}