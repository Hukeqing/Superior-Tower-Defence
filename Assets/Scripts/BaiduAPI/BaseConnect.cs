using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public enum TokenFetchStatus
{
    NotFetched,
    Fetching,
    Success,
    Failed
}
[Serializable]
class TokenResponse
{
    public string access_token = null;
}
public class BaseConnect : MonoBehaviour
{
    public string APIKey;                                               // API key
    public string SecretKey;                                            // Secret Key

    protected TokenFetchStatus CurStatus { get; private set; }
    protected string Tok;

    private UnityWebRequest WebRequest;

    public IEnumerator StartConnecting(Action<bool> SendBack)
    {
        if (CurStatus == TokenFetchStatus.Success)
        {
            SendBack(true);
        }
        else
        {
            yield return Connecting();
            SendBack(CurStatus == TokenFetchStatus.Success);
        }
    }
    public void BreakConnect() => WebRequest.Dispose();
    private IEnumerator Connecting()
    {
        CurStatus = TokenFetchStatus.Fetching;
        string uri = string.Format("https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={0}&client_secret={1}", APIKey, SecretKey);
        WebRequest = UnityWebRequest.Get(uri);
        //WebRequest.timeout = 100;
        yield return WebRequest.SendWebRequest();

        if (WebRequest.isHttpError || WebRequest.isNetworkError)
        {
#if UNITY_EDITOR
            Debug.Log("Connect Error: " + WebRequest.error);
#endif
            CurStatus = TokenFetchStatus.Failed;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Connect Success");
#endif
            CurStatus = TokenFetchStatus.Success;
            var result = JsonUtility.FromJson<TokenResponse>(WebRequest.downloadHandler.text);
            Tok = result.access_token;
        }
    }
}
