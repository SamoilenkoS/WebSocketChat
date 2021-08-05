namespace WebSocketChat.Commands
{
    public static class CommandHelper
    {
        public static Command GetCommand(string message)
        {
            Command result;
            bool isCommand = message.StartsWith('/');
            if (isCommand)
            {
                message = message.Substring(1);
                var spaceIndex = message.IndexOf(' ');
                if (spaceIndex != -1)
                {
                    var commandDescription = message.Substring(spaceIndex).TrimStart();
                    result = message switch
                    {
                        string str when str.StartsWith("msg") => new PrivateMessageCommand(commandDescription),
                        string str when str.StartsWith("nickname") => new NicknameChangeCommand(commandDescription),
                        _ => new InvalidCommand(string.Empty),
                    };
                }
                else
                {
                    result = new InvalidCommand(string.Empty);
                }
            }
            else
            {
                result = new MessageToAllCommand(message);
            }

            return result;
        }
    }
}
