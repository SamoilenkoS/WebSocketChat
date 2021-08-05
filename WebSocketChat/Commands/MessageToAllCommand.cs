using System.Threading.Tasks;
using WebSocketChat.SocketManager;

namespace WebSocketChat.Commands
{
    public class MessageToAllCommand : Command
    {
        public MessageToAllCommand(string message) : base(message)
        {
        }

        public override async Task Calculate(WebSocketClient sender, SocketHandler socketHandler)
        {
            await socketHandler.SendMessageToAll(Message);
        }
    }
}
