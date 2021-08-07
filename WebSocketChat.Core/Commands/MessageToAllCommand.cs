using System;
using System.Threading.Tasks;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.Core.Commands
{
    public class MessageToAllCommand : Command
    {
        private const int MinArgsCount = 1;

        private MessageToAllCommand(string[] args) : base(args)
        {
        }

        public static MessageToAllCommand Create(string[] args)
        {
            if (args.Length >= MinArgsCount)
            {
                return new MessageToAllCommand(args);
            }

            throw new ArgumentException("Message");//TODO remove it!
        }

        public override async Task ProcessMessage(WebSocketClient sender, SocketHandler socketHandler)
        {
            await socketHandler.SendMessageToAll(new MessageContract
            {
                Message = string.Join(' ', Args[0..]),
                ReceivedMessageColor = sender.MessagesColor
            },
            sender.Id);
        }
    }
}
