using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DiYi.Demo.Common
{
    /// <summary>
    /// web请求
    /// </summary>
    public sealed class WebUtil
    {
        private WebUtil() { }

        public static readonly HttpClient client;
        static WebUtil()
        {
            client = new HttpClient();
        }

        /// <summary>
        /// HttPClient操作POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpClientPost(string url, List<KeyValuePair<string, string>> data)
        {
            var content = new FormUrlEncodedContent(data);
            var response = client.PostAsync(url, content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }
        /// <summary>
        /// HttPClient操作POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpClientPostImg(string url, List<KeyValuePair<string, string>> data)
        {
            string str = string.Empty;
            if (data != null)
            {
                foreach (var item in data)
                {
                    str += item.Key + "=" + item.Value;
                }
            }
            var content = new StringContent(str, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string PostMoths(string url, string param)
        {
            Encoding encoding = Encoding.GetEncoding("GBK");
            string json = new JavaScriptSerializer().Serialize(new
            {
                text = param
            });
            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "application/json";
            //  string paraUrlCoded = val;
            var payload = encoding.GetBytes(json);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, encoding);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }
        /// <summary>
        /// HttPClient操作Get
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpClientGet(string url)
        {
            var response = client.GetAsync(url).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }

        /// <summary>
        /// 使用WebRequest进行post请求
        /// </summary>
        /// <param name="iServerURL"></param>
        /// <param name="iPostData"></param>
        /// <returns></returns>
        public static String PostWebRequest(String iServerURL, String iPostData)
        {
            String result = null;
            byte[] _buffer = Encoding.GetEncoding("utf-8").GetBytes(iPostData);
            HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(iServerURL);
            _req.Method = "Post";
            _req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            _req.ContentLength = _buffer.Length;
            Stream _stream = null;
            Stream _resStream = null;
            StreamReader _resSR = null;
            try
            {
                _stream = _req.GetRequestStream();
                _stream.Write(_buffer, 0, _buffer.Length);
                _stream.Flush();
                HttpWebResponse _res = (HttpWebResponse)_req.GetResponse();

                //获取响应
                _resStream = _res.GetResponseStream();
                _resSR = new StreamReader(_resStream, Encoding.GetEncoding("utf-8"));
                result = _resSR.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;

            }
            finally
            {
                if (_stream != null)
                {
                    _stream.Close();
                }
                if (_resSR != null)
                {
                    _resSR.Close();
                }
                if (_resStream != null)
                {
                    _resStream.Close();
                }
            }
            return result;
        }



    }
}
