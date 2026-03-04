using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
    public class User : IdentityUser
    {
        public ICollection<BorrowRecord>? BorrowRecords { get; set; } //借閱紀錄
        public string Role { get; set; } = "User";
        public string? Address { get; set; }
        public string MemberStatus { get; set; } = "Active";
        public DateTime JoinDate { get; set; } = DateTime.Now;
    }
}
