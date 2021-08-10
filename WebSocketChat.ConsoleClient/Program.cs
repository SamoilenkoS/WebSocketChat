using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebSocketChat.Core;

namespace WebSocketChat.ConsoleClient
{
    class Program
    {
        static void Main()
        {
            StartWebSockets().GetAwaiter().GetResult();
        }

        public static async Task StartWebSockets()
        {
            var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri("wss://websocketchatforitea.azurewebsites.net/ws"), CancellationToken.None);
            Console.WriteLine($"Web socket connection established @ {DateTime.UtcNow:F}");
            var send = Task.Run(async () =>
            {
                string message;
                while (!string.IsNullOrEmpty(message = Console.ReadLine()))
                {
                    var bytes = Encoding.Unicode.GetBytes(message);
                    await client.SendAsync(
                        new ArraySegment<byte>(bytes),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }

                await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            });

            var receive = ReceiveAsync(client);
            await Task.WhenAll(send, receive);
        }

        public static async Task ReceiveAsync(ClientWebSocket client)
        {
            var buffer = new byte[Consts.MessageSizeInBytes];
            WebSocketReceiveResult result;
            do
            {
                result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                var json = Encoding.Unicode.GetString(buffer, 0, result.Count);
                var messageContract = JsonSerializer.Deserialize<MessageContract>(json);

                ProcessMessage(messageContract);
            } while (result.MessageType != WebSocketMessageType.Close);
        }

        private static void ProcessMessage(MessageContract messageContract)
        {
            Console.ForegroundColor = messageContract.ReceivedMessageColor;
            Console.WriteLine(messageContract.Message);
            Console.ForegroundColor = messageContract.ClientMessageColor;
        }

    }
}
