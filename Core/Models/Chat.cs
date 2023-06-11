namespace Core.Models
{
    public class Chat : Entity
    {
        public User User { get; set; }
        public DateTime MessageDate { get; set; }
        public string Message { get; set; }

    }
}
