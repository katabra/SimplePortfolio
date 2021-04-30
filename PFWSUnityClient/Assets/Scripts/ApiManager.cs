using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 간단한 GET, POST 호출처리
/// </summary>
public class ApiManager : MonoBehaviour
{
    public TMPro.TMP_InputField addr;
    public Transform contentHolder;
    public GameObject content;

    public void GetLastChat()
    {
        StartCoroutine(CoApiCall(true, addr.text));
    }

    public void PostLastChat()
    {
        StartCoroutine(CoApiCall(false, addr.text));
    }

    IEnumerator CoApiCall(bool isGet, string uri)
    {
        using (
            UnityWebRequest www = 
            (isGet)?
            UnityWebRequest.Get(uri) :
            UnityWebRequest.Post(uri, string.Empty)
            )
        {
            yield return www.SendWebRequest();

            List<string> chatList = JsonConvert.DeserializeObject<List<string>>(www.downloadHandler.text);

            foreach(Transform t in contentHolder)
            {
                Destroy(t.gameObject);
            }

            foreach(string s in chatList)
            {
                GameObject newContent = Instantiate(content);
                newContent.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = s;
                newContent.transform.SetParent(contentHolder, false);
            }
        }
    }    
}
