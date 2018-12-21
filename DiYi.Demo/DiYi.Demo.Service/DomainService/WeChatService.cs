using DiYi.Demo.Common;
using DiYi.Demo.EntityDto;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.Service
{
    /// <summary>
    /// 调用微信接口
    /// </summary>
    public class WeChatService
    {
        /// <summary>
        /// 根据code 获取useraccesstoken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static UserAccessToken GetAccessToken(string code, string appid, string componenttoken, string componentappid)
        {
            StringBuilder urlbuilder = new StringBuilder("https://api.weixin.qq.com/sns/oauth2/component/access_token?");
            urlbuilder.AppendFormat("appid={0}", appid);
            urlbuilder.AppendFormat("&code={0}", code);
            urlbuilder.Append("&grant_type=authorization_code");
            urlbuilder.AppendFormat("&component_appid={0}", componentappid);
            urlbuilder.AppendFormat("&component_access_token={0}", componenttoken);

            string jsonText = WebUtil.HttpClientGet(urlbuilder.ToString());

            UserAccessToken token = JsonConvert.DeserializeObject<UserAccessToken>(jsonText);
            if (string.IsNullOrEmpty(token.access_token))
            {
                throw new Exception(jsonText);
            }
            return token;

        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static WeixinUserInfoResult GetUserInfo(string access_token, string openid)
        {
            WeixinUserInfoResult weixinUserInfoResult = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetUserInfo(access_token, openid);

            return weixinUserInfoResult;
            //StringBuilder url = new StringBuilder("https://api.weixin.qq.com/cgi-bin/user/info");
            //url.AppendFormat("?access_token={0}", access_token);
            //url.AppendFormat("&openid={0}", openid);
            //url.Append("&lang=zh_CN");
            //string jsonText = WebUtil.HttpClientGet(url.ToString());
            //return JsonConvert.DeserializeObject<WxUserInfo>(jsonText);
        }
    }
}
