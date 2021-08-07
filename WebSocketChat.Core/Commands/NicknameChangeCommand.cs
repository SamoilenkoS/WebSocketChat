using System.Threading.Tasks;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.Core.Commands
{
    public class NicknameChangeCommand : Command
    {
        public NicknameChangeCommand(string message) : base(message)
        {
        }

        public override async Task ProcessMessage(WebSocketClient sender, SocketHandler socketHandler)
        {
            var oldName = sender.ToString();
            sender.Nickname = Message;
            Message = string.Format(Consts.Messages.NicknameChangedMessage, oldName, sender.Nickname);

            await socketHandler.SendMessageToAll(
                new MessageContract
                {
                    Message = Message,
                    ReceivedMessageColor = sender.MessagesColor,
                    ClientMessageColor = sender.MessagesColor
                }, sender.Id);
        }
    }
}
