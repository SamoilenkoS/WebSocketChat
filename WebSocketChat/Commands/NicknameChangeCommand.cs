using System.Threading.Tasks;
using WebSocketChat.SocketManager;

namespace WebSocketChat.Commands
{
    public class NicknameChangeCommand : Command
    {
        public NicknameChangeCommand(string message) : base(message)
        {
        }

        public override async Task Calculate(WebSocketClient sender, SocketHandler socketHandler)
        {
            var oldName = sender.ToString();
            sender.Nickname = Message;
            Message = $"{oldName} nickname changed to {sender.Nickname}";

            await socketHandler.SendMessageToAll(Message);
        }
    }
}
