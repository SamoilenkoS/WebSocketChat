using System;
using System.Net.WebSockets;

namespace WebSocketChat.SocketManager
{
    public class WebSocketClient
    {
        private const string IdFormat = "N";

        public Guid Id { get; }

        public string Nickname { get; set; }

        public WebSocket WebSocket { get; }

        public WebSocketClient(WebSocket webSocket)
        {
            Id = Guid.NewGuid();
            WebSocket = webSocket;
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Nickname) ? Nickname : Id.ToString(IdFormat);
        }
    }
}
