using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketChat.SocketManager
{
    public class ConnectionManager : IEnumerable<Guid>
    {
        private const string ConnectionClosedMessage = "Socket connection closed";
        private const string IdFormat = "N";
        private readonly object _locker;
        private readonly List<WebSocketClient> _connections;

        public ConnectionManager()
        {
            _locker = new object();
            _connections = new List<WebSocketClient>();
        }

        public void AddSocket(WebSocket socket)
        {
            lock (_locker)
            {
                _connections.Add(new WebSocketClient(socket));
            }
        }

        public async Task RemoveSocketAsync(Guid id)
        {
            WebSocket socket;
            lock (_locker)
            {
                socket = _connections.FirstOrDefault(x => x.Id == id)?.WebSocket;
            }

            if (socket != null &&
                (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived))
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, ConnectionClosedMessage,
                    CancellationToken.None);
            }
        }

        public WebSocket GetSocketById(Guid id)
        {
            lock (_locker)
            {
                return _connections.FirstOrDefault(x => x.Id == id)?.WebSocket;
            }
        }

        public Guid GetId(WebSocket socket)
        {
            lock (_locker)
            {
                return _connections.FirstOrDefault(x => x.WebSocket == socket)?.Id ?? Guid.Empty;
            }
        }

        public WebSocketClient this[Guid id]
        {
            get
            {
                lock (_locker)
                {
                    return _connections.Find(x => x.Id == id);
                }
            }
        }

        public WebSocketClient this[WebSocket webSocket]
        {
            get
            {
                lock (_locker)
                {
                    return _connections.Find(x => x.WebSocket == webSocket);
                }
            }
        }

        public WebSocketClient this[string clientId]
        {
            get
            {
                lock (_locker)
                {
                    return _connections.Find(x => x.Nickname == clientId) ??
                           _connections.Find(x => x.Id.ToString(IdFormat) == clientId);
                }
            }
        }

        public IEnumerator<Guid> GetEnumerator()
        {
            foreach (var client in _connections)
            {
                yield return client.Id;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}