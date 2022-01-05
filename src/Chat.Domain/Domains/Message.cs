using System;

namespace Chat.Domain.Domains
{
    public class Message
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
