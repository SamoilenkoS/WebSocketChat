using System;

namespace WebSocketChat.Core
{
    public class MessageContract
    {
        public string Message { get; set; }
        public ConsoleColor ReceivedMessageColor { get; set; }
        public ConsoleColor ClientMessageColor { get; set; }
    }
}
