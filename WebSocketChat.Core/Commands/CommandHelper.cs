using System;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.Core.Commands
{
    public static class CommandHelper
    {
        public static Command GetCommand(string message)
        {
            Command result;
            if (!string.IsNullOrWhiteSpace(message))
            {
                bool isCommand = message.StartsWith(Consts.Commands.CommandSign);
                if (isCommand)
                {
                    message = message.Substring(1);
                    var commandArgs = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var commandName = commandArgs[0];
                    commandArgs = commandArgs[1..];

                    result = commandName switch
                    {
                        string str when str.StartsWith(Consts.Commands.PrivateMessageCommand)
                            => PrivateMessageCommand.Create(commandArgs),
                        string str when str.StartsWith(Consts.Commands.NicknameChangeCommand)
                            => NicknameChangeCommand.Create(commandArgs),
                        string str when str.StartsWith(Consts.Commands.ColorChangeCommand)
                            => ColorChangeCommand.Create(commandArgs),
                        _ => InvalidCommand.Create()
                    };
                }
                else
                {
                    result = MessageToAllCommand.Create(new[] { message });
                }
            }
            else
            {
                result = InvalidCommand.Create();
            }

            return result;
        }
    }
}
