using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketChat.SocketManager
{
    public class WebSocketMessageHandler : SocketHandler

    {
        public WebSocketMessageHandler(ConnectionManager connectionManager) : base(connectionManager)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = ConnectionManager.GetId(socket);

            await SendMessageToAll($"{socketId} just joined the party *****");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var webSocketClient = ConnectionManager[socket];

            await base.OnDisconnected(socket);
            await SendMessageToAll($"{webSocketClient} just left the party *****");
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var webSocketClient = ConnectionManager[socket];
            var messageText = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var message = $"{webSocketClient} said: {messageText}";
            if (messageText.StartsWith("/msg ", StringComparison.OrdinalIgnoreCase))//personal
            {
                messageText = messageText.Substring("/msg ".Length);
                var clientId = messageText.Substring(0, messageText.IndexOf(' '));
                var targetClient = ConnectionManager[clientId];

                if (targetClient != null)
                {
                    messageText = $"{targetClient} said to you: {messageText.Substring(clientId.Length)}";
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
