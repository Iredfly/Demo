using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.Common
{
    public class SignUtil
    {

        public static bool Validate(string token, string data, string signature)
        {
            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名数据
            var signStr = token + data;

            var bytes = Encoding.UTF8.GetBytes(signStr);
            //使用MD5加密
            var md5Val = hash.ComputeHash(bytes);
            //把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            foreach (var c in md5Val)
            {
                result.Append(c.ToString("X2"));
            }

            return result.ToString().ToUpper() == signature;
        }

        public static bool Validate(string token, IDictionary<string, string> stringDict, string signature)
        {
            string sign = Sign(stringDict, token);
            return sign == signature;
        }

        /// <summary>
        /// 参数签名,可用于微信支付请求签名
        /// </summary>
        /// <param name="stringADict"></param>
        /// <param name="key">对应微信支付的partnerkey或接口token</param>
        /// <returns></returns>
        public static string Sign(IDictionary<string, string> stringADict, string key)
        {
            var sb = new StringBuilder();
            foreach (var sA in stringADict.OrderBy(x => x.Key))//参数名ASCII码从小到大排序（字典序）；
            {
                if (string.IsNullOrEmpty(sA.Value)) continue;//参数的值为空不参与签名；
                sb.Append(sA.Key).Append("=").Append(sA.Value).Append("&");
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.Append("key=").Append(key);//在stringA最后拼接上key=(token值)得到stringSignTemp字符串

            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
            }
            var stringSignTemp = sb.ToString();
            //mb.AddLog("参与签名生成参数字符串：" + stringSignTemp);

            var sign = MD5(stringSignTemp, "UTF-8").ToUpper();//对stringSignTemp进行MD5运算，再将得到的字符串所有字符转换为大写，得到sign值signValue。 

            return sign;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string MD5(string encypStr, string charset = "UTF-8")
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }


        /// <summary>
        /// SHA1 加密，返回大写字符串
        /// </summary>
        /// <param name="content">需要加密字符串</param>
        /// <returns>返回40位UTF8 大写</returns>
        public static string SHA1(string content)
        {
            return SHA1(content, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string SHA1(string content, Encoding encode)
        {

            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(encode.GetBytes(content));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            string strSign = enText.ToString();
            return strSign;
        }

    }
}
