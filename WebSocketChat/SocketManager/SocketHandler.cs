using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketChat.SocketManager
{
    public abstract class SocketHandler
    {
        public ConnectionManager ConnectionManager { get; }

        public SocketHandler(ConnectionManager connectionManager)
        {
            ConnectionManager = connectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket)
            => await Task.Run(()
                => ConnectionManager.AddSocket(socket));

        public virtual async Task OnDisconnected(WebSocket socket)
            => await ConnectionManager.RemoveSocketAsync(ConnectionManager.GetId(socket));

        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State == WebSocketState.Open)
            {
                var encodedBytes = Encoding.Unicode.GetBytes(message);
                await socket.SendAsync(
                    new ArraySegment<byte>(encodedBytes, 0, encodedBytes.Length),
                    WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task SendMessage(Guid id, string message)
        {
            await SendMessage(ConnectionManager.GetSocketById(id), message);
        }

        public async Task SendMessageToAll(string message)
        {
            foreach (var connectionId in ConnectionManager)
            {
                await SendMessage(connectionId, message);
            }
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
