using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetRequestSender : MonoBehaviour
{
    public bool debugMode = false;

    public NetRequestStatus Status
    {
        get => status;
    }

    public bool Finished()
    {
        return status == NetRequestStatus.Error
            || status == NetRequestStatus.Success;
    }

    protected IEnumerator GetRequest(string uri)
    {
        status = NetRequestStatus.Running;
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        yield return webRequest.SendWebRequest();

        ProcessNetworkRequest(webRequest);
    }

    protected IEnumerator PostRequest(string url, WWWForm form)
    {
        status = NetRequestStatus.Running;
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
        yield return webRequest.SendWebRequest();

        ProcessNetworkRequest(webRequest);
    }

    protected IEnumerator PostRequestJson(string url, string json)
    {
        status = NetRequestStatus.Running;
        var webRequest = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        ProcessNetworkRequest(webRequest);
    }

    private void ProcessNetworkRequest(UnityWebRequest webRequest)
    {
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            status = NetRequestStatus.Error;
            if (debugMode) 
                Debug.Log("Error: " + webRequest.error);
        }
        else
        {
            status = NetRequestStatus.Success;
            if (debugMode) 
                Debug.Log("Received: " + webRequest.downloadHandler.text);
        }
    }

    protected NetRequestStatus status = NetRequestStatus.Idle;
}

public enum NetRequestStatus
{
    Idle,
    Running,
    Success,
    Error
}