namespace Library.Models
{
    public class Notify
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;//被通知人
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }
    }
}
