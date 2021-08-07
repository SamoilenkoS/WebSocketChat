using System.Threading.Tasks;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.Core.Commands
{
    public class InvalidCommand : Command
    {
        public InvalidCommand(string message) : base(message)
        {
        }

        public override async Task ProcessMessage(WebSocketClient sender, SocketHandler socketHandler)
        {
            await socketHandler.SendMessage(sender.WebSocket,
                new MessageContract
                {
                    Message = Consts.Messages.InvalidCommandMessage,
                    ReceivedMessageColor = sender.MessagesColor,
                    ClientMessageColor = sender.MessagesColor
                });
        }
    }
}
