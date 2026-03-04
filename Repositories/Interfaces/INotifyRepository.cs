using Library.Models;

namespace Library.Repositories.Interfaces
{
    public interface INotifyRepository
    {
        Task<IEnumerable<Notify>> GetByUserAsync(string userName);
        Task AddAsync(Notify notify);
    }
}
