using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebSocketChat.SocketManager
{
    public class SocketMiddleware
    {
        private readonly RequestDelegate _next;


        public SocketMiddleware(RequestDelegate next, SocketHandler handler)
        {
            _next = next;
            Handler = handler;
        }
        public SocketHandler Handler { get; set; }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();

            await Receive(socket, null/*messageToHandle*/);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> messageToHandle)
        {
            throw new NotImplementedException();
        }
    }
}