using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSocketChat.Commands;

namespace WebSocketChat.SocketManager
{
    public class WebSocketMessageHandler : SocketHandler
    {
        private const string JoinMessage = "{0} just joined the party *****";
        private const string LeaveMessage = "{0} just left the party *****";

        public WebSocketMessageHandler(ConnectionManager connectionManager) : base(connectionManager)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = ConnectionManager.GetId(socket);

            await SendMessageToAll(string.Format(JoinMessage, socketId));
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var webSocketClient = ConnectionManager[socket];

            await base.OnDisconnected(socket);
            await SendMessageToAll(string.Format(LeaveMessage, webSocketClient));
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var webSocketClient = ConnectionManager[socket];
            var messageText = Encoding.Unicode.GetString(buffer, 0, result.Count);
            await ProcessMessage(webSocketClient, messageText);
        }

        private async Task ProcessMessage(WebSocketClient senderSocket, string messageFromClient)
        {
            var command = CommandHelper.GetCommand(messageFromClient);
            if(command != null)
            {
                await command.Calculate(senderSocket, this);
            }
        }
    }
}
