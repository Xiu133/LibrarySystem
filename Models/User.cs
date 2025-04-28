using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
    public class User : IdentityUser
    {
        public ICollection<BorrowRecord>? BorrowRecords { get; set; } //借閱紀錄
        public string Role { get; set; } = "User";
    }
}
