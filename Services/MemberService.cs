using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Library.ViewModels;

namespace Library.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repo;
        public MemberService(IMemberRepository repo) => _repo = repo;

        public async Task<IEnumerable<User>> GetAllMembersAsync() => await _repo.GetAllAsync();

        public async Task<User?> GetMemberAsync(string userName) =>
            await _repo.GetByUserNameAsync(userName);

        public async Task<ServiceResult> SuspendAsync(string userName)
        {
            var user = await _repo.GetByUserNameAsync(userName);
            if (user == null) return ServiceResult.Fail("會員不存在。");
            user.MemberStatus = "Suspended";
            await _repo.UpdateAsync(user);
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> ActivateAsync(string userName)
        {
            var user = await _repo.GetByUserNameAsync(userName);
            if (user == null) return ServiceResult.Fail("會員不存在。");
            user.MemberStatus = "Active";
            await _repo.UpdateAsync(user);
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> UpdateAsync(User user)
        {
            var existing = await _repo.GetByUserNameAsync(user.UserName ?? string.Empty);
            if (existing == null) return ServiceResult.Fail("會員不存在。");
            existing.Address = user.Address;
            existing.PhoneNumber = user.PhoneNumber;
            await _repo.UpdateAsync(existing);
            return ServiceResult.Ok();
        }
    }
}
