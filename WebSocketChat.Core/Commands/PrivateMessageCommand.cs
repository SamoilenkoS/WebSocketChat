using System.Threading.Tasks;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.Core.Commands
{
    public class PrivateMessageCommand : Command
    {
        public PrivateMessageCommand(string message) : base(message)
        {
        }

        public override async Task ProcessMessage(WebSocketClient sender, SocketHandler socketHandler)
        {
            var spaceIndex = Message.IndexOf(' ');
            if(spaceIndex != -1)
            {
                var clientId = Message.Substring(0, spaceIndex);
                var targetClient = socketHandler.ConnectionManager[clientId];

                if (targetClient != null)
                {
                    Message = string.Format(
                        Consts.PrivateMessageFormat,
                        sender,
                        Message.Substring(clientId.Length + 1));

                    await socketHandler.SendMessage(targetClient.WebSocket, new MessageContract
                    {
                        Message = Message,
                        ReceivedMessageColor = sender.MessagesColor,
                        ClientMessageColor = targetClient.MessagesColor
                    });
                }
            }
        }
    }
}
