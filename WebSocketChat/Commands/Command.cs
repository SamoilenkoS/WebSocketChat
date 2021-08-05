using System.Threading.Tasks;
using WebSocketChat.SocketManager;

namespace WebSocketChat.Commands
{
    public abstract class Command
    {
        public string Message { get; protected set; }

        public Command(string message)
        {
            Message = message;
        }

        public abstract Task Calculate(WebSocketClient sender, SocketHandler socketHandler);
    }
}
