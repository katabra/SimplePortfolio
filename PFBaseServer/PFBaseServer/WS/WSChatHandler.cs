using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace PFBaseServer
{
    /// <summary>
    /// 웹소켓 쳇 처리 기능
    /// 단순히 받은 쳇을 전체로 뿌림
    /// </summary>
    public class WSChatHandler : WSHandler
    {
        WSChatQueue ChatQueue { get; set; }
        public WSChatHandler(WSConnectionManager wsManager, WSChatQueue chatQueue) : base(wsManager)
        {
            ChatQueue = chatQueue;
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
            ChatQueue.AddMsg(msg);
            await SendMessageToAllAsync(msg);
        }
    }
}
