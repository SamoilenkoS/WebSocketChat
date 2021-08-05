using System.Threading.Tasks;
using WebSocketChat.SocketManager;

namespace WebSocketChat.Commands
{
    public class PrivateMessageCommand : Command
    {
        public PrivateMessageCommand(string message) : base(message)
        {
        }

        public override async Task Calculate(WebSocketClient sender, SocketHandler socketHandler)
        {
            var spaceIndex = Message.IndexOf(' ');
            if(spaceIndex != -1)
            {
                var clientId = Message.Substring(0, spaceIndex);
                var targetClient = socketHandler.ConnectionManager[clientId];

                if (targetClient != null)
                {
                    Message = $"{sender} => {Message.Substring(clientId.Length + 1)}";
                    await socketHandler.SendMessage(targetClient.Id, Message);
                }
            }
        }
    }
}
