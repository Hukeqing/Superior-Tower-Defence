using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class TtsResponse
{
    public int err_no;
    public string err_msg;
    public string sn;
    public int idx;
    public bool Success => err_no == 0;
    public AudioClip clip;
}

public enum PerStatus
{
    Man, Woman, Mute, DuXiaoyao, DuYaya
}

public class TtsService : BaseConnect
{
    private const string UrlTts = "http://tsn.baidu.com/text2audio";

    private TtsResponse response;

    public IEnumerator GetAudio(string Txt, PerStatus per, int speed, int pit, int vol, Action<TtsResponse> callback)           //get audio
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            TtsResponse response = new TtsResponse
            {
                err_no = 999,
                err_msg = "Network Error"
            };
            callback(response);
            yield break;
        }
        yield return StartCoroutine(StartConnecting(IsConnect =>
        {
            if (IsConnect)
            {
                StartCoroutine(ttsGetAudio(Txt, per, speed, pit, vol, curttsResponse =>
                {
                    callback(curttsResponse);
                }));
            }
        }));
    }

    private IEnumerator ttsGetAudio(string Txt, PerStatus per, int speed, int pit, int vol, Action<TtsResponse> callback)
    {
        //tex：合成的文本，使用UTF - 8编码。小于512个中文字或者英文数字。（文本在百度服务器内转换为GBK后，长度必须小于1024字节）
        //tok：开放平台获取到的开发者access_token
        //cuid：用户唯一标识，用来区分用户，计算UV值。建议填写能区分用户的机器 MAC 地址或 IMEI 码，长度为60字符以内
        //ctp：客户端类型选择，web端填写固定值1
        //lan：固定值zh。语言选择,目前只有中英文混合模式，填写固定值zh
        //spd：语速，取值0 - 9，默认为5中语速
        //pit：音调，取值0 - 9，默认为5中语调
        //vol：音量，取值0 - 15，默认为5中音量
        //per：发音人选择, 0为普通女声，1为普通男生，3为情感合成 - 度逍遥，4为情感合成 - 度丫丫，默认为普通女声
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("tex", Txt);
        param.Add("tok", Tok);
        param.Add("cuid", SystemInfo.deviceUniqueIdentifier);
        param.Add("ctp", "1");
        param.Add("lan", "zh");
        param.Add("spd", Mathf.Clamp(speed, 0, 9).ToString());
        param.Add("pit", Mathf.Clamp(pit, 0, 9).ToString());
        param.Add("vol", Mathf.Clamp(vol, 0, 15).ToString());
        param.Add("per", ((int)per).ToString());
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_UWP
        param.Add("aue", "6"); // set to wav, default is mp3
#endif
        string url = UrlTts;
        int i = 0;
        foreach (var p in param)
        {
            url += i != 0 ? "&" : "?";
            url += p.Key + "=" + p.Value;
            i++;
        }
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_UWP
        UnityWebRequest WebRequest = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
#else
        UnityWebRequest WebRequest = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
#endif
        yield return WebRequest.SendWebRequest();

        if (!WebRequest.isHttpError && !WebRequest.isNetworkError)
        {
            var type = WebRequest.GetResponseHeader("Content-Type");

            if (type.Contains("audio"))
            {
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_UWP
                AudioClip clip = DownloadHandlerAudioClip.GetContent(WebRequest);
                TtsResponse response = new TtsResponse { clip = clip };
#else
                TtsResponse response = new TtsResponse {clip = DownloadHandlerAudioClip.GetContent(WebRequest) };
#endif
                callback(response);
            }
            else
            {
                byte[] textBytes = WebRequest.downloadHandler.data;
                var errorText = Encoding.UTF8.GetString(textBytes);
#if UNITY_EDITOR
                Debug.LogError("Error:" + errorText);
#endif
                callback(JsonUtility.FromJson<TtsResponse>(errorText));
            }
        }
    }
}
