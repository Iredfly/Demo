using DiYi.Demo.Common;
using DiYi.Demo.EntityDto;
using DiYi.Demo.EntityDto.Dto;
using DiYi.Demo.EntityDto.WxAuth;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DiYi.Demo.Api.Controllers
{
    /// <summary>
    /// 微信公共模块
    /// </summary>
    public class WechatController : BaseController
    {
        /// <summary>
        /// 获取微信jsapi配置
        /// </summary>
        /// <param name="jsapiConfigIn"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<WxConfigOut> JsapiConfig(JsapiConfigInDto jsapiConfigIn)
        {
            OutDto<WxConfigOut> baseOutDto = new OutDto<WxConfigOut>();

            string ticket = GetTicket(WechatAppId);
            string nonce_str = RandomNum.GenerateRandomNumber(16);
            string timestamp = TimeHelper.GetTimeStamp(DateTime.Now, 10);
            string wxTicket = "jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}";

            string strSign = SignUtil.SHA1(string.Format(wxTicket, ticket, nonce_str, timestamp, jsapiConfigIn.Url));

            string strResult = WechatAppId + " " + nonce_str + " " + timestamp + " " + strSign;

            WxConfigOut wxConfigOut = new WxConfigOut()
            {
                Url = jsapiConfigIn.Url,
                AppId = WechatAppId,
                nonceStr = nonce_str,
                RawString = strResult,
                Signature = strSign,
                Timestamp = timestamp,
                Ticket = ticket
            };
            baseOutDto.Data = wxConfigOut;
            baseOutDto.Code = (int)ResponseCode.Success;
            return baseOutDto;
        }
        /// <summary>                 
        /// 获取appid
        /// </summary>
        /// <param name="superIn"></param>
        /// <returns></returns>
        [HttpGet]
        public OutDto<string> AppId()
        {
            OutDto<string> res = new OutDto<string>();
            res.Data = WechatAppId;
            res.Message = "获取成功";
            return res;
        }

        /// <summary>
        /// 获取Ticket
        /// </summary>
        /// <param name="AppId"></param>
        /// <returns></returns>
        private string GetTicket(string AppId)
        {
            string key = "AuthorizerTToken:" + AppId;
            JsApiTicketResult tempTicket = redisService.Get<JsApiTicketResult>(key);
            if (tempTicket != null)
            {
                return tempTicket.ticket;
            }
            else
            {
                tempTicket = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetTicketByAccessToken(AuthorizerAToken(AppId));

                if (tempTicket.errcode == Senparc.Weixin.ReturnCode.请求成功)
                {
                    redisService.Insert<JsApiTicketResult>(key, tempTicket, tempTicket.expires_in);
                    return tempTicket.ticket;
                }
            }
            return "";
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="AppId"></param>
        /// <returns></returns>
        protected string AuthorizerAToken(string AppId)
        {
            string TokenKey = "AuthorizerAToken:" + AppId;
            TokenCache token = redisService.Get<TokenCache>(TokenKey);
            if (token != null)
            {
                return token.Token;
            }
            return string.Empty;
        }
    }
}
