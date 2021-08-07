using System;
using System.Net.WebSockets;

namespace WebSocketChat.Core.SocketManager
{
    public class WebSocketClient
    {
        public Guid Id { get; }

        public string Nickname { get; set; }

        public ConsoleColor MessagesColor { get; set; }

        public WebSocket WebSocket { get; }

        public WebSocketClient(WebSocket webSocket)
        {
            Id = Guid.NewGuid();
            WebSocket = webSocket;
            MessagesColor = ConsoleColor.Gray;
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Nickname) ? Nickname : Id.ToString(Consts.IdFormat);
        }
    }
}
