namespace Library.ViewModels
{
    public class MemberViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string MemberStatus { get; set; } = "Active";
        public string Role { get; set; } = "User";
        public DateTime JoinDate { get; set; }
        public int BorrowCount { get; set; }
    }
}
