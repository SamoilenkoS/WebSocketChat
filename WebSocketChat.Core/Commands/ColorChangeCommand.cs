using System;
using System.Threading.Tasks;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.Core.Commands
{
    public class ColorChangeCommand : Command
    {
        private const int ArgsCount = 1;

        private ColorChangeCommand(string[] args) : base(args)
        {
        }

        public static ColorChangeCommand Create(string[] args)
        {
            if (args.Length == ArgsCount)
            {
                return new ColorChangeCommand(args);
            }

            throw new ArgumentException(
                string.Format(Consts.Messages.InvalidCommandArgumentsMessage, nameof(ColorChangeCommand), ArgsCount));
        }

        public override async Task ProcessMessage(WebSocketClient sender, SocketHandler socketHandler)
        {
            if (Enum.TryParse<ConsoleColor>(Args[0], out var newColor))
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
