using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleChat.Messaging.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}
