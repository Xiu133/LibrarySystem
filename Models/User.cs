using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Library.Models
{
    public class User
    {
        public int Id { get; set; }
        [Key]
        public string UserName { get; set; } = string.Empty; //帳號
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<BorrowRecord>? BorrowRecords { get; set; } //借閱紀錄

    }
}
