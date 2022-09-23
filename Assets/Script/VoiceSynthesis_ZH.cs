using Baidu.Aip.Speech;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 语音合成 
/// </summary>
public class VoiceSynthesis_ZH : MonoBehaviour
{
 
    [Header("音源")]
    public AudioSource _Audio;

    [Header("AI 发声器")]
    public Pronouncer _Pronouncer = Pronouncer.Duyaya;

    //全局变量
    public static VoiceSynthesis_ZH _World;

    //网页文字转语音
    private string _Url;

    //百度语音识别SDK
    private Asr _AipClient;



    private void Start()
    {
        _World = this;
    }


    //获取 Web网页音源信息并播放
    public IEnumerator GetAudioClip(string AudioText)
    {
        //_Url = "https://tsn.baidu.com/text2audio?tex=" + AudioText + 
        //  "+&lan=zh&cuid=7919875968150074&ctp=1&aue=6&tok=25.3141e5ae3aa109abb6fc9a8179131181.315360000.1886566986.282335-17539441";

        _Url = "http://tsn.baidu.com/text2audio?tex=" + AudioText +
              "&tok=25.3141e5ae3aa109abb6fc9a8179131181.315360000.1886566986.282335-17539441" +
              "&cuid=a7a0e3326da873c6fb0609e6385a82b934c9cb11" +
              "&ctp=1" +
              "&lan=zh" +
              "&spd=5" +
              "&pit=5" +
              "&vol=10" +
              "&per=" + (((int)_Pronouncer).ToString()) +
              "&aue=6";


        using (UnityWebRequest _AudioWeb = UnityWebRequestMultimedia.GetAudioClip(_Url, AudioType.WAV))
        {

            yield return _AudioWeb.SendWebRequest();
            if (_AudioWeb.isNetworkError)
            {
                yield break;
            }
            AudioClip _Cli = DownloadHandlerAudioClip.GetContent(_AudioWeb);
            _Audio.clip = _Cli;
            _Audio.Play();
        }
    }

	/// <summary>
    /// 获取 Web网页音源信息并播放 附带延迟时间
    /// </summary>
    /// <param 播放文字="AudioText"></param>
    /// <param 延迟时间="_DelayedTimer"></param>
    /// <returns></returns>
    public IEnumerator GetAudioClip(string AudioText,float _DelayedTimer)
    {
        //_Url = "https://tsn.baidu.com/text2audio?tex= AudioText &lan=zh&cuid=7919875968150074&ctp=1&aue=6&tok=25.3141e5ae3aa109abb6fc9a8179131181.315360000.1886566986.282335-17539441
        //  "";
        yield return new WaitForSeconds(_DelayedTimer);


        _Url = "http://tsn.baidu.com/text2audio?tex=" + AudioText +
              "&tok=25.3141e5ae3aa109abb6fc9a8179131181.315360000.1886566986.282335-17539441" +
              "&cuid=a7a0e3326da873c6fb0609e6385a82b934c9cb11" +
              "&ctp=1" +
              "&lan=zh" +
              "&spd=5" +
              "&pit=5" +
              "&vol=10" +
              "&per=" + (((int)_Pronouncer).ToString()) +
              "&aue=6";


        using (UnityWebRequest _AudioWeb = UnityWebRequestMultimedia.GetAudioClip(_Url, AudioType.WAV))
        {

            yield return _AudioWeb.SendWebRequest();
            if (_AudioWeb.isNetworkError)
            {
                yield break;
            }
            AudioClip _Cli = DownloadHandlerAudioClip.GetContent(_AudioWeb);
            //_ASRMusic.clip = _Cli;


            //_ASRMusic.Play();
        }
    }

    /// <summary>
    /// 语音识别
    /// </summary>
    /// <param 录取音频 = "_Clip" ></ param >
    /// < returns ></ returns >
   private IEnumerator Recognition(AudioClip _Clip)
    {
        //开放 音频 长度
        float[] _Sample = new float[_Clip.samples];

        //用片段中的样本数据填充数组
        _Clip.GetData(_Sample, 0);

        //数据转换
        short[] _IntData = new short[_Sample.Length];
        byte[] _ByteData = new byte[_IntData.Length * 2];
        for (int i = 0; i < _Sample.Length; i++)
        {
            _IntData[i] = (short)(_Sample[i] * short.MaxValue);
        }
        Buffer.BlockCopy(_IntData, 0, _ByteData, 0, _ByteData.Length);

        //返回Json数据  （数据 格式 码率）
        var _Result = _AipClient.Recognize(_ByteData, "pcm", 16000);

        //获取Json 数据中的 讲话内容
        var _Speaking = _Result.GetValue("result");

        //检测是否有内容
        if (_Speaking == null)
        {
            StopAllCoroutines();
            yield return null;
        }

        //讲话内容转换为 字符串
        string _UsefulText = _Speaking.First.ToString();
        print(_UsefulText);


        yield return 0;
    }


    /// <summary>
    /// 访问令牌获取
    /// </summary>
    /// <returns></returns>
    private static class AccessToken

    {
        // 调用getAccessToken()获取的 access_token建议根据expires_in 时间 设置缓存
        // 返回token示例
        public static String TOKEN = "24.adda70c11b9786206253ddb70affdc46.2592000.1493524354.282335-1234567";

        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        private static String clientId = "百度云应用的AK";
        // 百度云中开通对应服务应用的 Secret Key 
        private static String clientSecret = "百度云应用的SK";

        public static String GetAccessToken()
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<string, string>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            return result;
        }
    }

    /// <summary>
    /// AI 发音器
    /// </summary>
    public enum Pronouncer
    {
        //普通女声
        Female,
        //普通男生
        Male,
        //特殊男声
        Teshunan,
        //情感合成男生
        Duxiaoyao,
        //情感合成女生
        Duyaya
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

