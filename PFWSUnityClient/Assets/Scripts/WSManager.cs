using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 간단한 웹소켓 쳇 처리
/// 별개의 플러그인을 사용하지 않고 닷넷의 기본제공 웹소켓 클라이언트 사용
/// </summary>
public class WSManager : MonoBehaviour
{
    public TMPro.TMP_InputField address;
    public TMPro.TMP_InputField input;
    public Transform contentHolder;
    public GameObject content;
    public UnityEngine.UI.Scrollbar vScroll;

    ClientWebSocket ws;
    Queue<string> msgQueue = new Queue<string>();

    private void Start()
    {
    }

    private void Update()
    {
        if (msgQueue.Count > 0)
        {
            string msg = msgQueue.Dequeue();
            GameObject newContent = Instantiate(content);
            newContent.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = msg;
            newContent.transform.SetParent(contentHolder, false);
            StartCoroutine(CoVerticalScrollUpdate());
        }
    }

    IEnumerator CoVerticalScrollUpdate()
    {
        yield return null;
        yield return null;

        vScroll.value = 0;
    }

    private void OnDestroy()
    {
        ws?.Dispose();
    }

    public void StartClient()
    {
        _ = Client();
    }

    async Task Client()
    {
        using (ws = new ClientWebSocket())
        {
            byte[] buffer = new byte[4096];

            Uri uri = new Uri(address.text);// "ws://echo.websocket.org");
            await ws.ConnectAsync(uri, CancellationToken.None);
            while(ws.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                switch(result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        string msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        msgQueue.Enqueue(msg);
                        break;
                    case WebSocketMessageType.Close:
                        return;
                }
            }
        }
    }

    public void SendMsg()
    {
        string msg = input.text;
        ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
        input.text = "";
    }

}
