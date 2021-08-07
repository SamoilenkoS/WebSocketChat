using System;
using System.Threading.Tasks;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.Core.Commands
{
    public class ColorChangeCommand : Command
    {
        public ColorChangeCommand(string message) : base(message)
        {
        }

        public override async Task ProcessMessage(WebSocketClient sender, SocketHandler socketHandler)
        {
            if (Enum.TryParse<ConsoleColor>(Message, out var newColor))
            {
                sender.MessagesColor = newColor;
                await socketHandler.SendMessage(sender.WebSocket, new MessageContract
                {
                    Message = "Color changed",
                    ClientMessageColor = sender.MessagesColor,
                    ReceivedMessageColor = sender.MessagesColor
                });
            }
        }
    }
}
