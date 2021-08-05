using System.Threading.Tasks;
using WebSocketChat.SocketManager;

namespace WebSocketChat.Commands
{
    public class InvalidCommand : Command
    {
        public InvalidCommand(string message) : base(message)
        {
        }

        public override async Task Calculate(WebSocketClient sender, SocketHandler socketHandler)
        {
            await socketHandler.SendMessage(sender.WebSocket, "Invalid command!");
        }
    }
}
