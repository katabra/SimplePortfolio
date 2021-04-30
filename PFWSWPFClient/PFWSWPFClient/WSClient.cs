using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PFWSWPFClient
{
    /// <summary>
    /// 닷넷 기본제공 웹소켓 클라이언트 사용
    /// </summary>
    class WSClient
    {
        ClientWebSocket ws;

        public async Task Connect(string addr)
        {
            if (ws != null)
                ws.Dispose();

            ws = new ClientWebSocket();
            byte[] buffer = new byte[4096];

            Uri uri = new Uri(addr);
            await ws.ConnectAsync(uri, CancellationToken.None);

            while(ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                
                MainWindow.main.AddNewChatList(msg);
            }
        }

        public async Task SendAsync(string msg)
        {
            if(ws?.State == WebSocketState.Open)
                await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
