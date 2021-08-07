using System.Threading.Tasks;

namespace WebSocketChat.Core.SocketManager
{
    public abstract class Command
    {
        public string[] Args { get; protected set; }

        protected Command(string[] args)
        {
            Args = args;
        }

        public abstract Task ProcessMessage(WebSocketClient sender, SocketHandler socketHandler);
    }
}
