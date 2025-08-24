using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Library.Models
{
    public class BorrowRule
    {
        [Key]
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public int MaxBooks { get; set; }
    }
}
