namespace WebSocketChat.Core
{
    public static class Consts
    {
        public static class Messages
        {
            public const string InvalidCommandMessage = "Invalid command!";
            public const string NicknameChangedMessage = "{0} nickname changed to {1}";
            public const string ConnectionClosedMessage = "Socket connection closed";
            public const string JoinMessage = "{0} just joined the party *****";
            public const string LeaveMessage = "{0} just left the party *****";
            public const string InvalidCommandArgumentsMessage = "Command \"{0}\" required {1} args.";
        }

        public static class Commands
        {
            public const char CommandSign = '/';
            public const string PrivateMessageCommand = "msg";
            public const string NicknameChangeCommand = "nickname";
            public const string ColorChangeCommand = "color";
        }

        public const string PrivateMessageFormat = "{0} => {1}";
        public const string IdFormat = "N";
        public const int MessageSizeInBytes = 1024 * 4;
    }
}
