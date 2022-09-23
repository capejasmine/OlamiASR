using Ai.Olami.Example;
using Baidu.Aip.Speech;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Json_ZH;
using Toggle = UnityEngine.UI.Toggle;

/// <summary>
/// 人工智能语音播放
/// </summary>
public class ASR_ZH : MonoBehaviour
{
    [Header("文字显示")]
    public Text _ASRText;

    [Header("ASR Toggle")]
    public Toggle _Toggle;
    
    public Text _Label;

    public GameObject _LoadingBlock;
    
    [Header("ASR Json")]
    private Root _JsonDataWeb;

    [Header("ASR Json 故事")]
    private Root02 _JsonDataWeb02;

    [Header("ASR Json 百度百科")]
    private Root03 _JsonDataWeb03;

    [Header("ASR Json 选择")]
    private Root04 _JsonDataWeb04;

    [Header("ASR Json 菜肴")]
    private Root05 _JsonDataWeb05;

    [Header("ASR Json 搜索")]
    private Root06 _JsonDataWeb06;

    [Header("ASR Json 股票")]
    private Root07 _JsonDataWeb07;

    [Header("ASR Json 电视节目")]
    private Root08 _JsonDataWeb08;

    [Header("ASR Json 天气")]
    private Root09 _JsonDataWeb09;

    [Header("ASR Json 诗集选择")]
    private Root10 _JsonDataWeb10;

    [Header("ASR Json 菜肴选择")]
    private Root11 _JsonDataWeb11;

    [Header("ASR Json 酒店选择")]
    private Root12 _JsonDataWeb12;

    [Header("ASR Json 诗集")]
    private Root13 _JsonDataWeb13;

    //测试文本
    private string _Str = "天气如何";

    //欧拉密 Key 值
    private string _Key= "cf18fd35e4fd4e9aa9585b7fb7e0220c";
    //欧拉密 Secret 值
    private string _Secret = "85484fbb6b81412eb04e3fe6e0b21827";
    //欧拉密访问网址
    private string _ApiBaseUrl = "https://cn.olami.ai/cloudservice/api";




    //语音片段
    private AudioClip _RecordClip;

    //访问AIP需要用的Token
    private string _AccessToken;
    //百度语音 ID
    private string _ASR_ID = "27594203";
    //百度语音 Key
    private string _API_Key = "l4LFMKZobAMV1adGghdf1DtQ";
    //百度语音 Secret
    private string _Secret_Key = "dk5PzYNKY4XrV9GjG3kfMZMy4KPspFXC";
    //百度语音 访问网址
    private string _AuthHost = "https://aip.baidubce.com/oauth/2.0/token";

    //百度语音识别SDK
    private Asr _AipClient;

    //重置中
    private bool _isResetRecord = false;

    void Start()
    {
       // 创建SDK的实例
       _AipClient = new Asr(_ASR_ID, _API_Key, _Secret_Key);
        //超时时长为6000毫秒
        _AipClient.Timeout = 6000;
        //保存当前应用的Token
       _AccessToken = GetAccessToken();

        //Toggle 事件添加
        _Toggle.onValueChanged.AddListener((bool ison) => { ToggleIs(_Toggle); });

    }

    /// <summary>
    /// 重置录音状态
    /// </summary>
    public void ResetRecord()
    {
        print("-------------重置状态-------------");
        _isResetRecord = true;
        _Label.text = "开始录音";
        _Toggle.isOn = false;
    }

    /// <summary>
    /// 点击按下说话开始录音
    /// </summary>
    private int pressCount = 0;
    public void StartRecord()
    {
        //麦克风查询
        if (Microphone.devices.Length > 0 && pressCount != 0)
        {
            //麦克风获取
            string _Device = Microphone.devices[0];

            print(_Device);
            //确认音频格式 麦克风   超过录音时长是否继续录音 录音时间(秒)  由录音产生的AudioClip的采样率
          _RecordClip = Microphone.Start(_Device, true, 60, 16000);//8000 16000  16000

        }
        else
        {
            _ASRText.text = "没有音频输入模块，请重新尝试";

            print("没有音频输入模块。");
            ResetRecord();
        }

        pressCount++;
    }

    /// <summary>
    /// 松开按下说话停止录音并发送识别
    /// </summary>
    private void StopRecord()
    {
        if (_isResetRecord)
        {
            _isResetRecord = false;
            return;
        }

        //麦克风停止响应
        Microphone.End(Microphone.devices[0]);
        //开始识别
        StartCoroutine(Recognition(_RecordClip));
    }

    /// <summary>
    /// 设置识别文本
    /// </summary>
    /// <param 返回值 = "_Result" ></ param >
    private void SetRecognizeText(string _Result)
    {
        _ASRText.text = _Result;
    }

    /// <summary>
    /// 访问令牌获取
    /// </summary>
    /// <returns></returns>
    private string GetAccessToken()
    {
        //百度AI 里面的

        HttpClient _Client = new HttpClient();
        List<KeyValuePair<string, string>> _ParaList = new List<KeyValuePair<string, string>>();
        _ParaList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
        _ParaList.Add(new KeyValuePair<string, string>("client_id", _API_Key));
        _ParaList.Add(new KeyValuePair<string, string>("client_secret", _Secret_Key));

        HttpResponseMessage _Response = _Client.PostAsync(_AuthHost, new FormUrlEncodedContent(_ParaList)).Result;
        string _Result = _Response.Content.ReadAsStringAsync().Result;
        Debug.Log("result is " + _Result);

        return _Result;

    }



    /// <summary>
    /// 语音识别
    /// </summary>
    /// <param 录取音频 = "_Clip" ></ param >
    /// < returns ></ returns >
    private IEnumerator Recognition(AudioClip _Clip)
    {
        SetBlock(true);

        yield return null;
        yield return null;
        
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

        JObject _Result;
        try
        {
            //返回Json数据  （数据 格式 码率）
            _Result = _AipClient.Recognize(_ByteData, "pcm", 16000);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            SetBlock(false);
            throw;
        }
        
        //获取Json 数据中的 讲话内容
        var _Speaking = _Result.GetValue("result");

        //检测是否有内容
        if (_Speaking == null)
        {
            SetBlock(false);
            SetRecognizeText("没有录到声音");
            StopAllCoroutines();
            yield return null;
        }

        //讲话内容转换为 字符串
        string _UsefulText = _Speaking.First.ToString();
        print("识别输入： " + _UsefulText);

        //开启人工智能回答
        StartCoroutine(OLAMIStr(_UsefulText));

        yield return 0;
    }

    /// <summary>
    /// Toggle 控件
    /// </summary>
    /// <param Toggle="_ToggleASR"></param>
    public void ToggleIs(Toggle _ToggleASR)
    {
        if (_ToggleASR.isOn)
        {
            print("-------------开始录音-------------");
            _Label.text = "停止录音";
            //开始识别
            StartRecord();
        }
        else
        {
            print("-------------停止录音-------------");
            _Label.text = "开始录音";
            //停止录音
            StopRecord();
        }
    }

    /// <summary>
    /// 欧拉密 人工智能回答
    /// </summary>
    /// <param 问题 = "_ASRText" ></ param >
    /// < returns ></ returns >
    private IEnumerator OLAMIStr(string _ASRText)
    {

        //欧拉米返回Json数据
        string _OLAMIStr = GetRecognitionResult(NluApiSample.API_NAME_NLI, _ASRText);
        
        SetBlock(false);

        //Json 数据WebGL 数据加载
        //Json 数据类 创建
        _JsonDataWeb = new Root();
        _JsonDataWeb = JsonUtility.FromJson<Root>(_OLAMIStr);

        //确保J送数据不为空
        if (_JsonDataWeb != null)
        {
            //如果欧拉密饭会数据 是error的话
            if (_JsonDataWeb.status == "error")
            {
                SetRecognizeText("解析失败，请重试。");
                //播放语音
                StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip("解析失败，请重试。"));
            }
            else
            {
                //获取 欧拉密回答返回值
                for (int i = 0; i < _JsonDataWeb.data.nli.Count; i++)
                {
                    //欧拉密返回值 类型
                    switch (_JsonDataWeb.data.nli[i].type)
                    {
                        #region 故事模块
                        case "joke":

                            print("故事模块");

                            //Json 数据WebGL 数据加载
                            //Json 数据类 创建
                            _JsonDataWeb02 = new Root02();
                            _JsonDataWeb02 = JsonUtility.FromJson<Root02>(_OLAMIStr);

                            //字符串优化
                            StringBuilder _SpeciesJoke = new StringBuilder();

                            if (_JsonDataWeb02.data.nli.Count > 0)
                            {


                                for (int j = 0; j < _JsonDataWeb02.data.nli.Count; j++)
                                {
                                    _SpeciesJoke.Append(_JsonDataWeb02.data.nli[j].desc_obj.result + "\n");

                                    _SpeciesJoke.Append(_JsonDataWeb02.data.nli[j].data_obj[j].content);
                                    //屏幕输出返回值
                                    SetRecognizeText(_SpeciesJoke.ToString());
                                }

                                //播放人工智能语音 
                                StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesJoke.ToString()));
                            }
                            else
                            {
                                _SpeciesJoke.Append("你可以换个说法哟。");
                                //播放人工智能语音 
                                StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesJoke.ToString()));
                            }
                            break;
                        #endregion


                        #region 百度百科模块
                        case "baike":

                            print("百度百科模块");

                            //Json 数据WebGL 数据加载
                            //Json 数据类 创建
                            _JsonDataWeb03 = new Root03();
                            _JsonDataWeb03 = JsonUtility.FromJson<Root03>(_OLAMIStr);

                            for (int k = 0; k < _JsonDataWeb03.data.nli.Count; k++)
                            {

                                //字符串优化
                                StringBuilder _SpeciesBaike = new StringBuilder();

                                //如果是  [生物] 物种 类
                                if (_JsonDataWeb03.data.nli[k].data_obj[k].type == "species")
                                {
                                    _SpeciesBaike.Append(_JsonDataWeb03.data.nli[k].desc_obj.result + "\n" + "\n");

                                    //确保数据存在
                                    if (_JsonDataWeb03.data.nli[k].data_obj[k].field_value.Count>0)
                                    {
                                        //具体信息
                                        for (int j = 0; j < _JsonDataWeb03.data.nli[k].data_obj[k].field_value.Count; j++)
                                        {
                                            //名称 + 内容
                                            _SpeciesBaike.Append(_JsonDataWeb03.data.nli[k].data_obj[k].field_name[j] + "：" + _JsonDataWeb03.data.nli[k].data_obj[k].field_value[j] + "\n");
                                        }
                                    }
                                    else
                                    {
                                        _SpeciesBaike.Append("没有找到具体数据");
                                    }



                                    _SpeciesBaike.Append("\n" + "详细信息：" + _JsonDataWeb03.data.nli[k].data_obj[k].description);

                                    //百度百科具体内容
                                    SetRecognizeText(_SpeciesBaike.ToString());


                                    //播放人工智能语音 
                                    //StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_JsonDataWeb03.data.nli[k].desc_obj.result));

                                    //延时执行
                                    //yield return new WaitForSeconds(2.5f);

                                    //播放人工智能语音 
                                    StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesBaike.ToString()));


                                }
                                else
                                {
                                    //欧拉密回答
                                    _SpeciesBaike.Append(_JsonDataWeb03.data.nli[k].desc_obj.result + "\n");
                                    //确保数据存在
                                    if (_JsonDataWeb03.data.nli[k].data_obj[k].field_value.Count > 0)
                                    {
                                        //具体信息
                                        for (int j = 0; j < _JsonDataWeb03.data.nli[k].data_obj[k].field_value.Count; j++)
                                        {
                                            //名称 + 内容
                                            _SpeciesBaike.Append(_JsonDataWeb03.data.nli[k].data_obj[k].field_name[j] + "：" + _JsonDataWeb03.data.nli[k].data_obj[k].field_value[j] + "\n");
                                        }
                                        
                                    }
                                    else
                                    {
                                        _SpeciesBaike.Append("没有找到具体数据");
                                    }
                                    //详细信息
                                    _SpeciesBaike.Append(_JsonDataWeb03.data.nli[k].data_obj[k].description);

                                    //百度百科具体内容
                                    SetRecognizeText(_SpeciesBaike.ToString());

                                    //播放人工智能语音 
                                    //StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_JsonDataWeb03.data.nli[k].desc_obj.result));

                                    //延时执行
                                    //yield return new WaitForSeconds(2.5f);



                                    //播放人工智能语音 
                                    StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesBaike.ToString()));
                                }


                            }
                            break;
                        #endregion


                        #region 选择模块
                        case "selection":

                            print("选择模块");

                            #region 多轮对话选择模块

                            switch (_JsonDataWeb.data.nli[i].desc_obj.type)
                            {
                                case "poem":

                                    print("诗集模块");

                                    //Json 数据WebGL 数据加载
                                    //Json 数据类 创建
                                    _JsonDataWeb10 = new Root10();
                                    _JsonDataWeb10 = JsonUtility.FromJson<Root10>(_OLAMIStr);


                                    //字符串优化
                                    StringBuilder _SpeciesSelectionPoem = new StringBuilder();

                                    _SpeciesSelectionPoem.Append(_JsonDataWeb10.data.nli[i].desc_obj.result + "\n\n");

                                    //确保数据存在
                                    if (_JsonDataWeb10.data.nli[i].data_obj.Count > 0)
                                    {
                                        for (int j = 0; j < _JsonDataWeb10.data.nli[i].data_obj.Count; j++)
                                        {
                                            _SpeciesSelectionPoem.Append("《" + _JsonDataWeb10.data.nli[i].data_obj[j].poem_name + "》" + "\n");
                                            _SpeciesSelectionPoem.Append("作者：" + _JsonDataWeb10.data.nli[i].data_obj[j].author + "\n\n");
                                        }
                                    }
                                    else
                                    {
                                        _SpeciesSelectionPoem.Append("没有找到哟");
                                    }

                                    //百度百科具体内容 第一次对话
                                    SetRecognizeText(_SpeciesSelectionPoem.ToString());

                                    //播放人工智能语音 
                                    StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesSelectionPoem.ToString()));
                                    break;


                                case "cooking":

                                    print("菜肴选择模块");

                                    //Json 数据WebGL 数据加载
                                    //Json 数据类 创建
                                    _JsonDataWeb11 = new Root11();
                                    _JsonDataWeb11 = JsonUtility.FromJson<Root11>(_OLAMIStr);


                                    //字符串优化
                                    StringBuilder _SpeciesSelectionCooking = new StringBuilder();

                                    _SpeciesSelectionCooking.Append(_JsonDataWeb11.data.nli[i].desc_obj.result + "\n\n");

                                    //确保数据存在
                                    if (_JsonDataWeb11.data.nli[i].data_obj.Count > 0)
                                    {
                                        for (int j = 0; j < _JsonDataWeb11.data.nli[i].data_obj.Count; j++)
                                        {
                                            _SpeciesSelectionCooking.Append("名称：" + _JsonDataWeb11.data.nli[i].data_obj[j].name + "\n\n");
                                        }
                                    }
                                    else
                                    {
                                        _SpeciesSelectionCooking.Append("没有找到这道菜，可以换一个说法。");
                                    }

                                    //百度百科具体内容 第一次对话
                                    SetRecognizeText(_SpeciesSelectionCooking.ToString());

                                    //播放人工智能语音 
                                    StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesSelectionCooking.ToString()));

                                    break;


                                case "hotel":

                                    print("酒店选择模块");

                                    //Json 数据WebGL 数据加载
                                    //Json 数据类 创建
                                    _JsonDataWeb12 = new Root12();
                                    _JsonDataWeb12 = JsonUtility.FromJson<Root12>(_OLAMIStr);


                                    //字符串优化
                                    StringBuilder _SpeciesHotel = new StringBuilder();

                                    _SpeciesHotel.Append(_JsonDataWeb12.data.nli[i].desc_obj.result + "\n\n");

                                    //确保数据存在
                                    if (_JsonDataWeb12.data.nli[i].data_obj.Count > 0)
                                    {
                                        for (int j = 0; j < _JsonDataWeb12.data.nli[i].data_obj.Count; j++)
                                        {
                                            _SpeciesHotel.Append("酒店名字：" + _JsonDataWeb12.data.nli[i].data_obj[j].hotel_name + "\n");
                                            _SpeciesHotel.Append("酒店描述：" + _JsonDataWeb12.data.nli[i].data_obj[j].description + "\n");
                                            _SpeciesHotel.Append("酒店地址：" + _JsonDataWeb12.data.nli[i].data_obj[j].hotel_address + "\n");
                                            _SpeciesHotel.Append("酒店形象：" + _JsonDataWeb12.data.nli[i].data_obj[j].hotel_image + "\n");
                                            _SpeciesHotel.Append("平均价格：" + _JsonDataWeb12.data.nli[i].data_obj[j].max_price + "\n");
                                            _SpeciesHotel.Append("最低价格：" + _JsonDataWeb12.data.nli[i].data_obj[j].floor_price + "\n");
                                            _SpeciesHotel.Append("酒店网站：" + _JsonDataWeb12.data.nli[i].data_obj[j].description_url + "\n");
                                            _SpeciesHotel.Append("携程评级：" + _JsonDataWeb12.data.nli[i].data_obj[j].ctrip_rating + "\n");
                                            _SpeciesHotel.Append("用户评级：" + _JsonDataWeb12.data.nli[i].data_obj[j].user_rating + "\n\n");
                                        }
                                    }
                                    else
                                    {
                                        _SpeciesHotel.Append("可以尝试再说一遍。");
                                    }

                                    //百度百科具体内容 第一次对话
                                    SetRecognizeText(_SpeciesHotel.ToString());

                                    //播放人工智能语音 
                                    StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesHotel.ToString()));

                                    break;

                                default:

                                    //百度百科具体内容 第一次对话
                                    SetRecognizeText("对不起，我没有明白你说的是什么意思？");

                                    //播放人工智能语音 
                                    StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip("对不起，我没有明白你说的是什么意思？"));
                                    break;
                            }

                            #endregion

                            break;
                        #endregion


                        #region 菜肴模块
                        case "cooking":

                            print("菜肴模块");

                            //Json 数据WebGL 数据加载
                            //Json 数据类 创建
                            _JsonDataWeb05 = new Root05();
                            _JsonDataWeb05 = JsonUtility.FromJson<Root05>(_OLAMIStr);

                            //字符串优化
                            StringBuilder _SpeciesCooking = new StringBuilder();

                            if (_JsonDataWeb05.data.nli.Count > 0)
                            {
                                for (int j = 0; j < _JsonDataWeb05.data.nli.Count; j++)
                                {
                                    //欧拉密响应
                                    _SpeciesCooking.Append(_JsonDataWeb05.data.nli[j].desc_obj.result + "\n");
                                    //具体菜肴做法
                                    _SpeciesCooking.Append(_JsonDataWeb05.data.nli[j].data_obj[j].content);
                                }
                            }
                            else
                            {
                                _SpeciesCooking.Append("没有听清你在说什么。");
                            }

                            //屏幕输出返回值  
                            SetRecognizeText(_SpeciesCooking.ToString());
                            //播放人工智能语音 
                            StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesCooking.ToString()));
                            break;
                        #endregion


                        #region 搜索模块
                        case "openweb":

                            print("搜索模块");

                            //Json 数据WebGL 数据加载
                            //Json 数据类 创建
                            _JsonDataWeb06 = new Root06();
                            _JsonDataWeb06 = JsonUtility.FromJson<Root06>(_OLAMIStr);

                            //字符串优化
                            StringBuilder _SpeciesOpenweb = new StringBuilder();

                            if (_JsonDataWeb06.data.nli.Count > 0)
                            {
                                for (int j = 0; j < _JsonDataWeb06.data.nli.Count; j++)
                                {
                                    //欧拉密响应
                                    _SpeciesOpenweb.Append(_JsonDataWeb06.data.nli[j].desc_obj.result + "\n");

                                    //具体搜索信息
                                    _SpeciesOpenweb.Append(_JsonDataWeb06.data.nli[j].data_obj[j].url);

                                    //播放人工智能语音 
                                    StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_JsonDataWeb06.data.nli[j].desc_obj.result));
                                }
                            }
                            else
                            {
                                _SpeciesOpenweb.Append("可以描述的再具体一点。");
                            }

                            //屏幕输出返回值  
                            SetRecognizeText(_SpeciesOpenweb.ToString());
                            break;
                        #endregion


                        #region 股票模块
                        case "stock":

                            print("股票模块");

                            //Json 数据WebGL 数据加载
                            //Json 数据类 创建
                            _JsonDataWeb07 = new Root07();
                            _JsonDataWeb07 = JsonUtility.FromJson<Root07>(_OLAMIStr);

                            //字符串优化
                            StringBuilder _SpeciesStock = new StringBuilder();

                            if (_JsonDataWeb07.data.nli.Count > 0)
                            {
                                for (int j = 0; j < _JsonDataWeb07.data.nli.Count; j++)
                                {
                                    //如果有股票数据的话
                                    if (_JsonDataWeb07.data.nli[j].data_obj.Count > 0)
                                    {
                                        //欧拉密响应
                                        _SpeciesStock.Append(_JsonDataWeb07.data.nli[j].desc_obj.result + "\n");

                                        _SpeciesStock.Append("股票名称：" + _JsonDataWeb07.data.nli[j].data_obj[j].name + "\n");
                                        _SpeciesStock.Append("股票代码：" + _JsonDataWeb07.data.nli[j].data_obj[j].id + "\n");
                                        _SpeciesStock.Append("最高点：" + _JsonDataWeb07.data.nli[j].data_obj[j].price_high + "\n");
                                        _SpeciesStock.Append("最低点：" + _JsonDataWeb07.data.nli[j].data_obj[j].price_low + "\n");
                                        _SpeciesStock.Append("今开：" + _JsonDataWeb07.data.nli[j].data_obj[j].price_start + "\n");
                                        _SpeciesStock.Append("昨收：" + _JsonDataWeb07.data.nli[j].data_obj[j].price_end + "\n");
                                        _SpeciesStock.Append("变化率：" + _JsonDataWeb07.data.nli[j].data_obj[j].change_rate + "\n");
                                        _SpeciesStock.Append("最低交易：" + _JsonDataWeb07.data.nli[j].data_obj[j].cur_price + "\n");
                                        _SpeciesStock.Append("成交量：" + _JsonDataWeb07.data.nli[j].data_obj[j].volume + "\n");
                                    }
                                    else
                                    {
                                        _SpeciesStock.Append(_JsonDataWeb07.data.nli[j].desc_obj.result + "\n");
                                    }
                                    //播放人工智能语音 
                                    StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_JsonDataWeb07.data.nli[0].desc_obj.result));

                                }
                                //屏幕输出返回值  
                                SetRecognizeText(_SpeciesStock.ToString());

                                //播放人工智能语音 
                                //StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesStock.ToString()));

                            }
                            break;
                        #endregion


                        #region 电视模块
                        case "tvprogram":

                            print("电视模块");

                            //Json 数据WebGL 数据加载
                            //Json 数据类 创建
                            _JsonDataWeb08 = new Root08();
                            _JsonDataWeb08 = JsonUtility.FromJson<Root08>(_OLAMIStr);

                            //字符串优化
                            StringBuilder _SpeciesTvprogram = new StringBuilder();

                            if (_JsonDataWeb08.data.nli.Count > 0)
                            {
                                for (int j = 0; j < _JsonDataWeb08.data.nli.Count; j++)
                                {
                                    //欧拉密响应
                                    _SpeciesTvprogram.Append(_JsonDataWeb08.data.nli[j].desc_obj.result + "\n");
                                    //节目单数据填充
                                    for (int k = 0; k < _JsonDataWeb08.data.nli[j].data_obj.Count; k++)
                                    {
                                        //具体搜索信息
                                        _SpeciesTvprogram.Append(_JsonDataWeb08.data.nli[j].data_obj[k].name + "\n");
                                        _SpeciesTvprogram.Append("播出时间是：" + _JsonDataWeb08.data.nli[j].data_obj[k].time + "\n" + "\n");
                                    }
                                }
                            }

                            //屏幕输出返回值  
                            SetRecognizeText(_SpeciesTvprogram.ToString());
                            //播放人工智能语音 
                            StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesTvprogram.ToString())); 
                            break;
                        #endregion


                        #region 天气模块
                        case "weather":

                            print("天气模块");

                            //Json 数据WebGL 数据加载
                            //Json 数据类 创建
                            _JsonDataWeb09 = new Root09();
                            _JsonDataWeb09 = JsonUtility.FromJson<Root09>(_OLAMIStr);

                            //字符串优化
                            StringBuilder _SpeciesWeather = new StringBuilder();

                            if (_JsonDataWeb09.data.nli.Count > 0)
                            {
                                for (int j = 0; j < _JsonDataWeb09.data.nli.Count; j++)
                                {
                                    //欧拉密响应
                                    _SpeciesWeather.Append(_JsonDataWeb09.data.nli[j].desc_obj.result + "\n");
                                    //节目单数据填充
                                    for (int k = 0; k < _JsonDataWeb09.data.nli[j].data_obj.Count; k++)
                                    {
                                        //具体搜索信息
                                        _SpeciesWeather.Append(_JsonDataWeb09.data.nli[j].data_obj[k].description + "\n");
                                    }
                                }                               
                            }
                            else
                            {
                                _SpeciesWeather.Append("你的声音太小了，我没听清楚。");
                            }


                            //屏幕输出返回值  
                            SetRecognizeText(_SpeciesWeather.ToString());
                            //播放人工智能语音 
                            StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesWeather.ToString()));
                            break;
                        #endregion


                        #region 诗集模块 
                        case "poem":

                            print("诗集模块");

                            //Json 数据WebGL 数据加载
                            //Json 数据类 创建
                            _JsonDataWeb13 = new Root13();
                            _JsonDataWeb13 = JsonUtility.FromJson<Root13>(_OLAMIStr);

                            //字符串优化
                            StringBuilder _SpeciesPoem = new StringBuilder();

                            if (_JsonDataWeb13.data.nli.Count > 0)
                            {
                                for (int j = 0; j < _JsonDataWeb13.data.nli.Count; j++)
                                {
                                    //确保数据存在
                                    if (_JsonDataWeb13.data.nli[j].data_obj.Count > 0)
                                    {
                                        for (int k = 0; k < _JsonDataWeb13.data.nli[j].data_obj.Count; k++)
                                        {
                                            //具体搜索信息
                                            _SpeciesPoem.Append("《" + _JsonDataWeb13.data.nli[j].data_obj[k].title + "》" + "\n");
                                            _SpeciesPoem.Append("作者：" + _JsonDataWeb13.data.nli[j].data_obj[k].author + "\n\n");
                                            _SpeciesPoem.Append(_JsonDataWeb13.data.nli[j].data_obj[k].content + "\n");
                                        }

                                    }
                                    else
                                    {
                                        _SpeciesPoem.Append("我没有找到哦，你可以再说一遍。");
                                    }

                                }
                            }
                            else
                            {
                                _SpeciesPoem.Append("你的声音太小了，我没听清楚。");
                            }


                            //屏幕输出返回值  
                            SetRecognizeText(_SpeciesPoem.ToString());
                            //播放人工智能语音 
                            //播放人工智能语音 
                            StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_SpeciesPoem.ToString()));
                            break;
                        #endregion


                        default:

                            print("基础模块");
                            //屏幕输出返回值
                            SetRecognizeText(_JsonDataWeb.data.nli[i].desc_obj.result);

                            //播放人工智能语音 
                            StartCoroutine(VoiceSynthesis_ZH._World.GetAudioClip(_JsonDataWeb.data.nli[i].desc_obj.result));

                            break;
                    }
                }
            }
        }
        yield return 0;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            print($"测试文本： {_Str}");
            StartCoroutine(OLAMIStr(_Str));
        }
    }



    /// <summary>
    /// 欧拉密令牌访问 和数据返回  
    /// </summary>
    /// <param HTTP参数 API="_ApiName">HTTP参数“API = nli”的API名称</param>
    /// <param 输入文本="_InputText">你想要识别的文本</param>
    /// <returns>识别结果</returns>
    private string GetRecognitionResult(string _ApiName, string _InputText)
    {
        string _Result;
        double _Timestamp = Convert.ToInt64((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
        
        // 准备消息以生成MD5摘要
        StringBuilder _SignMsg = new StringBuilder()
            .Append(_Secret)
            .Append("api=")
            .Append(_ApiName)
            .Append("appkey=")
            .Append(_Key)
            .Append("timestamp=")
            .Append(_Timestamp)
            .Append(_Secret);

        // 生成MD5摘要
        MD5 _Md5 = MD5.Create();
        StringBuilder _Sign = new StringBuilder();
        byte[] hash = _Md5.ComputeHash(Encoding.Default.GetBytes(_SignMsg.ToString()));
        for (int i = 0; i < hash.Length; i++)
        {
            _Sign.Append(hash[i].ToString("X2"));
        }

        // 通过HTTP POST请求NLU服务
        using (WebClient _Client = new WebClient())
        {
            print(("发送'POST'请求到URL: " + _ApiBaseUrl));

            NameValueCollection _ApiParam = null;

            //如果 HTTP API 为 seg
            if (_ApiName == NluApiSample.API_NAME_SEG)
            {
                _ApiParam = new NameValueCollection()
                    {
                        { "appkey", _Key },
                        { "api", NluApiSample.API_NAME_SEG },
                        { "timestamp", _Timestamp.ToString() },
                        { "sign", _Sign.ToString() },
                        { "rq", _InputText }
                    };
            }
            //如果 HTTP API 为 nli
            else if (_ApiName == NluApiSample.API_NAME_NLI)
            {
                //https://cn.olami.ai/cloudservice/api?
                //appkey=your_app_key&
                //api=nli&
                //timestamp=current_timestamp&
                //sign=your_sign&
                //rq={"data":{"userValidLibs":[{"config":{"domain":"ST_USER","name":"ST_USER_通讯录"},"data":"张三|三仔\n李四|小四|四仔"}]},"data_type":"config"}&cusid=yourDeviceID

                _ApiParam = new NameValueCollection()
                    {
                        { "appkey", _Key },
                        { "api", NluApiSample.API_NAME_NLI },
                        { "timestamp", _Timestamp.ToString() },
                        { "sign", _Sign.ToString() },
                        { "rq", "{\"data_type\":\"stt\", \"data\":{\"input_type\":1,\"text\":\"" + _InputText + "\"}}" }
                    };
            }

            try
            {
                //获取网页数据
                byte[] _Response = _Client.UploadValues(_ApiBaseUrl, _ApiParam);
                //byte 数组 转换为 文字
                _Result = Encoding.UTF8.GetString(_Response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                SetBlock(false);
                throw;
            }
        }

        print(_Result);
        //返回欧拉密 Json 数据
        return _Result;     
    }

    private void SetBlock(bool enable)
    {
        _LoadingBlock.gameObject.SetActive(enable);
    }

    #region 百度令牌访问弃用

    private IEnumerator GetAccessToken02()
    {
        WWWForm wWWForm = new WWWForm();
        wWWForm.AddField("grant_type", "client_credentials");
        wWWForm.AddField("client_id", _API_Key);
        wWWForm.AddField("client_secret", _Secret_Key);

        UnityWebRequest unityWebRequest = UnityWebRequest.Post(_AuthHost, wWWForm);

        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError)
        {
            print("网络错误");
        }
        else
        {
            if (unityWebRequest.responseCode == 200)
            {
                string re = unityWebRequest.downloadHandler.text;
                print("获取：" + re);
                //onGet
            }
        }
    }


    #endregion
}

