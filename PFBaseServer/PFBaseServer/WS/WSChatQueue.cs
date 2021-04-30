using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFBaseServer
{
    /// <summary>
    /// 단순히 받은 문자열을 큐에 저장, 최대 5개
    /// </summary>
    public class WSChatQueue
    {
        LinkedList<string> chatQueue = new LinkedList<string>();

        public void AddMsg(string msg)
        {
            chatQueue.AddLast(msg);
            while (chatQueue.Count > 5)
                chatQueue.RemoveFirst();
        }

        public List<string> GetLastMsgs()
        {
            return chatQueue.ToList();
        }
    }
}
