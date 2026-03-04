using Library.Models;
using Library.ViewModels;

namespace Library.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<User>> GetAllMembersAsync();
        Task<User?> GetMemberAsync(string userName);
        Task<ServiceResult> SuspendAsync(string userName);
        Task<ServiceResult> ActivateAsync(string userName);
        Task<ServiceResult> UpdateAsync(User user);
    }
}
