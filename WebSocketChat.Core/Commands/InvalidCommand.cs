using System.Threading.Tasks;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.Core.Commands
{
    public class InvalidCommand : Command
    {
        private InvalidCommand(string[] args) : base(args)
        {
        }

        public static InvalidCommand Create()
        {
            return new InvalidCommand(null);
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
