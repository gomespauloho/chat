namespace Chat.Infrastructure.Entities
{
    public class MessageEntity : Entity
    {
        public string Username { get; set; }
        public string Content { get; set; }
    }
}
