using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto
{
    /// <summary>
    /// 
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 请求(或处理)成功
        /// </summary>
        Success = 200,

        /// <summary>
        /// 请求(或处理)失败
        /// </summary>
        Fail = 0,

        /// <summary>
        /// 内部请求出错
        /// </summary>
        Error = 500,

        /// <summary>
        /// 请求参数不完整或不正确
        /// </summary>
        ParameterError = 400,

        /// <summary>
        /// 未授权标识
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 授权参数不足
        /// </summary>
        AuthParameterError = 402,

        /// <summary>
        /// 请求TOKEN失效
        /// </summary>
        TokenInvalid = 403,

        /// <summary>
        /// HTTP请求类型不合法
        /// </summary>
        HttpMehtodError = 405,

        /// <summary>
        /// HTTP请求不合法
        /// </summary>
        HttpRequestError = 406,

        /// <summary>
        /// HTTP请求不合法
        /// </summary>
        URLExpireError = 407,

        /// <summary>
        /// 缺少签名
        /// </summary>
        NoSignature = 408
    }
}
