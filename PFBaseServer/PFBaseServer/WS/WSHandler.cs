using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PFBaseServer
{
    /// <summary>
    /// 웹소켓 처리 추상 클래스
    /// </summary>
    public abstract class WSHandler
    {
        protected WSConnectionManager WebSocketConnectionManager { get; set; }

        public WSHandler(WSConnectionManager wsManager)
        {
            WebSocketConnectionManager = wsManager;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            WebSocketConnectionManager.AddSocket(socket);
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
        }

        public async Task SendMessageAsync(WebSocket socket, string msg)
        {
            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task SendMessageAsync(string id, string msg)
        {
            await SendMessageAsync(WebSocketConnectionManager.GetSocketById(id), msg);
        }

        public async Task SendMessageToAllAsync(string msg)
        {
            foreach (var p in WebSocketConnectionManager.GetAll())
            {
                await SendMessageAsync(p.Value, msg);
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
