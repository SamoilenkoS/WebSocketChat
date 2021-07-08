﻿using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebSocketChat.SocketManager
{
    public class SocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SocketHandler Handler;

        public SocketMiddleware(RequestDelegate next, SocketHandler handler)
        {
            _next = next;
            Handler = handler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();

            await Handler.OnConnected(socket);

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await Handler.Receive(socket, result, buffer);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await Handler.OnDisconnected(socket);
                }
            });
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> messageToHandle)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    messageToHandle(result, buffer);
                }
                catch (WebSocketException ex)
                {
                    if (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        await Handler.OnDisconnected(socket);
                    }
                }
            }
        }
    }
}