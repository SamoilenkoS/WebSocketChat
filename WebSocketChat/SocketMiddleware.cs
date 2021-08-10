using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebSocketChat.Core;
using WebSocketChat.Core.SocketManager;

namespace WebSocketChat.SocketManager
{
    public class SocketMiddleware
    {
        private readonly SocketHandler _socketHandler;
        private readonly ILogger<SocketMiddleware> _logger;

        public SocketMiddleware(RequestDelegate _, SocketHandler handler, ILogger<SocketMiddleware> logger)
        {
            _socketHandler = handler;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();

                await _socketHandler.OnConnected(socket);

                await Receive(socket);
            }
        }

        private async Task Receive(WebSocket socket)
        {
            var buffer = new byte[Consts.MessageSizeInBytes];
            while (socket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    await ProcessMessageBySocket(socket, result, buffer);
                }
                catch (WebSocketException ex)
                {
                    if (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        await _socketHandler.OnDisconnected(socket);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        private async Task ProcessMessageBySocket(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            if (result.MessageType == WebSocketMessageType.Text)
            {
                await _socketHandler.Receive(socket, result, buffer);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await _socketHandler.OnDisconnected(socket);
            }
        }
    }
}