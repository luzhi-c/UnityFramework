using GameFramework;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityGameFramework.Runtime
{
    public static class WebRequestExtra
    {
        public static void Request(string webRequestUri, string postData, GameFrameworkAction<string> succ, GameFrameworkAction fail)
        {
            var m_UnityWebRequest = new UnityWebRequest(webRequestUri, "POST");
            m_UnityWebRequest.uploadHandler = new UploadHandlerRaw(Utility.Converter.GetBytes(postData));
            m_UnityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            UnityWebRequestAsyncOperation op = m_UnityWebRequest.SendWebRequest();
            op.completed +=
            (AsyncOperation ao) =>
            {
                bool isError = m_UnityWebRequest.result == UnityWebRequest.Result.ConnectionError || m_UnityWebRequest.result == UnityWebRequest.Result.ProtocolError;
                if (isError)
                {
                    fail?.Invoke();
                }
                else
                {
                    if (m_UnityWebRequest.downloadHandler.isDone)
                    {
                        succ?.Invoke(Utility.Converter.GetString(m_UnityWebRequest.downloadHandler.data));
                    }
                }
            };

        }
    }
}