using System.Threading.Tasks;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.Core.Commands
{
    public class MessageToAllCommand : Command
    {
        public MessageToAllCommand(string message) : base(message)
        {
        }

        public override async Task ProcessMessage(WebSocketClient sender, SocketHandler socketHandler)
        {
            await socketHandler.SendMessageToAll(new MessageContract
            {
                Message = Message,
                ReceivedMessageColor = sender.MessagesColor
            },
            sender.Id);
        }
    }
}
