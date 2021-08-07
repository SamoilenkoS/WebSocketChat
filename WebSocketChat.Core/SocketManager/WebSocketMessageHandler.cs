using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSocketChat.Core.Commands;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.Core
{
    public class WebSocketMessageHandler : SocketHandler
    {
        public WebSocketMessageHandler(ConnectionManager connectionManager) : base(connectionManager)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var webSocketClient = ConnectionManager[socket];

            await SendMessageToAll(
                new MessageContract
                {
                    Message = string.Format(Consts.Messages.JoinMessage, webSocketClient.Id),
                    ReceivedMessageColor = webSocketClient.MessagesColor,
                    ClientMessageColor = webSocketClient.MessagesColor
                }, webSocketClient.Id);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var webSocketClient = ConnectionManager[socket];

            await base.OnDisconnected(socket);
            await SendMessageToAll(
                new MessageContract
                {
                    Message = string.Format(Consts.Messages.LeaveMessage, webSocketClient),
                    ReceivedMessageColor = webSocketClient.MessagesColor,
                    ClientMessageColor = webSocketClient.MessagesColor
                }, webSocketClient.Id);
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
                await command.ProcessMessage(senderSocket, this);
            }
        }
    }
}
