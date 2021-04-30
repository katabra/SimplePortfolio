using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace PFBaseServer
{
    /// <summary>
    /// 간단 웹소켓 처리 미들웨어
    /// </summary>
    public class WSManagerMiddle
    {
        readonly RequestDelegate _next;
        WSHandler _wsHandler { get; set; }

        public WSManagerMiddle(RequestDelegate next, WSHandler wsHandler)
        {
            _next = next;
            _wsHandler = wsHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await _wsHandler.OnConnected(socket);

            await Receive(socket, 
                async (result, buffer) => {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        await _wsHandler.ReceiveAsync(socket, result, buffer);
                    }
                    else if(result.MessageType == WebSocketMessageType.Close)
                    {
                        await _wsHandler.OnDisconnected(socket);
                    }
                }
            );
        }
        async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                handleMessage(result, buffer);
            }
            await _wsHandler.OnDisconnected(socket);
        }
    }
}
