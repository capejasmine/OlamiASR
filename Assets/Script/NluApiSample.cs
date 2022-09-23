/*
	Copyright 2017, VIA Technologies, Inc. & OLAMI Team.

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace Ai.Olami.Example
{
    class NluApiSample
    {
        public static string API_NAME_SEG = "seg";
        public static string API_NAME_NLI = "nli";

        private string _ApiBaseUrl;
        private string _AppKey;
        private string _AppSecret;

        /// <summary>
        /// 设置访问OLAMI服务的授权信息
        /// </summary>
        /// <param _AppKey="appKey">从OLAMI开发人员控制台获得的AppKey  </param>
        /// <param _AppSecret="appSecret">从OLAMI开发控制台的AppSecret  </param>
        public void SetAuthorization(string appKey, string appSecret)
        {
            this._AppKey = appKey;
            this._AppSecret = appSecret;
        }

        /// <summary>
        /// 设置本地化以选择服务区域，这与不同的服务器url或语言等有关。  
        /// </summary>
        /// <param _ApiBaseUrl="apiBaseURL">API服务的URL</param>
        public void SetLocalization(string apiBaseURL)
        {
            this._ApiBaseUrl = apiBaseURL;
        }

        /// <summary>
        /// 获取输入文本的NLU识别结果。  
        /// </summary>
        /// <param name="_ApiName">HTTP参数“API =xxx”的API名称</param>
        /// <param name="_InputText">你想要识别的文本</param>
        /// <returns>识别结果</returns>
        public string GetRecognitionResult(string _ApiName, string _InputText)
        {
            
            string _Result;
			double _Timestamp = Convert.ToInt64((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);

            // 准备消息以生成MD5摘要
            StringBuilder _SignMsg = new StringBuilder()
                .Append(_AppSecret)
                .Append("api=")
                .Append(_ApiName)
                .Append("appkey=")
                .Append(_AppKey)
                .Append("timestamp=")
                .Append(_Timestamp)
                .Append(_AppSecret);

            // 生成MD5摘要
            MD5 _Md5 = MD5.Create();
            StringBuilder _Sign = new StringBuilder();
            byte[] _Hash = _Md5.ComputeHash(Encoding.Default.GetBytes(_SignMsg.ToString()));
            for (int i = 0; i < _Hash.Length; i++)
            {
                _Sign.Append(_Hash[i].ToString("X2"));
            }

            // 通过HTTP POST请求NLU服务
            using (WebClient _Client = new WebClient())
            {
                Console.WriteLine("Sending 'POST' request to URL : " + _ApiBaseUrl);

                NameValueCollection _ApiParam = null;

                if (_ApiName == NluApiSample.API_NAME_SEG)
                {
                    _ApiParam = new NameValueCollection()
                    {
                        { "appkey", _AppKey },
                        { "api", NluApiSample.API_NAME_SEG },
                        { "timestamp", _Timestamp.ToString() },
                        { "sign", _Sign.ToString() },
                        { "rq", _InputText }
                    };
                }
                else if (_ApiName == NluApiSample.API_NAME_NLI)
                {
                    _ApiParam = new NameValueCollection()
                    {
                        { "appkey", _AppKey },
                        { "api", NluApiSample.API_NAME_NLI },
                        { "timestamp", _Timestamp.ToString() },
                        { "sign", _Sign.ToString() },
                        { "rq", "{\"data_type\":\"stt\", \"data\":{\"input_type\":1,\"text\":\"" + _InputText + "\"}}" }
                    };
                }

                byte[] _Response = _Client.UploadValues(_ApiBaseUrl, _ApiParam);

                _Result = System.Text.Encoding.UTF8.GetString(_Response);
            }

            return _Result;
        }
    }
}

