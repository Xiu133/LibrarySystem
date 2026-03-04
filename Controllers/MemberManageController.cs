using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MemberManageController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberManageController(IMemberService memberService) => _memberService = memberService;

        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllMembersAsync();
            return View(members);
        }

        [HttpPost]
        public async Task<IActionResult> Suspend(string userName)
        {
            var result = await _memberService.SuspendAsync(userName);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public async Task<IActionResult> Activate(string userName)
        {
            var result = await _memberService.ActivateAsync(userName);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> GetMemberDetail(string userName)
        {
            var member = await _memberService.GetMemberAsync(userName);
            if (member == null) return Json(null);
            return Json(new
            {
                userName = member.UserName,
                email = member.Email,
                phoneNumber = member.PhoneNumber,
                address = member.Address,
                role = member.Role,
                memberStatus = member.MemberStatus,
                joinDate = member.JoinDate.ToString("yyyy-MM-dd")
            });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string userName)
        {
            var member = await _memberService.GetMemberAsync(userName);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            return Json(members.Select(m => new
            {
                m.UserName,
                m.Role,
                m.MemberStatus,
                m.PhoneNumber,
                JoinDate = m.JoinDate.ToString("yyyy-MM-dd"),
                m.Email
            }));
        }
    }
}
