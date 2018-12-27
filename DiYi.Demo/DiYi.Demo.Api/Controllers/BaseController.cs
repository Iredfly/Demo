using DiYi.Demo.Service;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DiYi.Demo.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : ApiController
    {
        /// <summary>
        /// 微信第三方APPID
        /// </summary>
        public static string WxBaseAppId = ConfigurationManager.AppSettings["WeixinAppID"] ?? "";
        /// <summary>
        /// 测试使用公众号appId
        /// </summary>
        public static string WechatAppId = ConfigurationManager.AppSettings["WechatAppId"] ?? "";

        public static RedisService redisService = new RedisService();
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
}
