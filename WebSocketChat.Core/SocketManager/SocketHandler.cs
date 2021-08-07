using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketChat.Core.SocketManager
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

        public async Task SendMessage(WebSocket socket, MessageContract messageContract)
        {
            if (socket.State == WebSocketState.Open)
            {
                var message = JsonSerializer.Serialize(messageContract);
                var encodedBytes = Encoding.Unicode.GetBytes(message);
                await socket.SendAsync(
                    new ArraySegment<byte>(encodedBytes, 0, encodedBytes.Length),
                    WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task SendMessageToAll(MessageContract messageContract, Guid senderId)
        {
            foreach (var webSocketClient in ConnectionManager)
            {
                if(webSocketClient.Id != senderId)
                {
                    messageContract.ClientMessageColor = webSocketClient.MessagesColor;
                    await SendMessage(webSocketClient.WebSocket, messageContract);
                }
            }
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
