using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

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
            var message = $"{webSocketClient}: {messageText}";
            if (messageText.StartsWith("/msg ", StringComparison.OrdinalIgnoreCase))//personal
            {
                messageText = messageText.Substring("/msg ".Length);
                var clientId = messageText.Substring(0, messageText.IndexOf(' '));
                var targetClient = ConnectionManager[clientId];

                if (targetClient != null)
                {
                    messageText = $"{webSocketClient} => {messageText.Substring(clientId.Length + 1)}";
                    await SendMessage(targetClient.Id, messageText);
                    return;
                }
            }
            else if (messageText.StartsWith("/nickname ", StringComparison.OrdinalIgnoreCase))//nickname change
            {
                messageText = messageText.Substring("/nickname ".Length);
                if (!messageText.Contains(" "))
                {
                    var oldName = webSocketClient.ToString();
                    webSocketClient.Nickname = messageText;
                    message = $"{oldName} nickname changed to {messageText}";
                }
            }

            await SendMessageToAll(message);
        }
    }
}
