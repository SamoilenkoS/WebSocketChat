using System.Threading.Tasks;

namespace WebSocketChat.Core.SocketManager
{
    public abstract class Command
    {
        public string Message { get; protected set; }

        public Command(string message)
        {
            Message = message;
        }

        public abstract Task ProcessMessage(WebSocketClient sender, SocketHandler socketHandler);
    }
}
