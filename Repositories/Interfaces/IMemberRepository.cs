using Library.Models;
namespace Library.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByUserNameAsync(string userName);
        Task<bool> UpdateAsync(User user);
    }
}
