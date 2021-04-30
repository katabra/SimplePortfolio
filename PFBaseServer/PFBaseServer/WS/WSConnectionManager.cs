using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace PFBaseServer
{
    /// <summary>
    /// 간단 소켓 관리 클래스
    /// </summary>
    public class WSConnectionManager
    {        
        ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(x => x.Key == id).Value;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(x => x.Value == socket).Key;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll() => _sockets;

        public void AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
        }

        public async Task RemoveSocket(string id)
        {
            WebSocket socket;
            _sockets.TryRemove(id, out socket);

            await socket?.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed By Manager", CancellationToken.None);
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
