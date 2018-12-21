using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto
{
    /// <summary>
    /// 请求日志类型
    /// </summary>
    public enum RequestLogType
    {
        None = -1,

        /// <summary>
        /// 后台请求   
        /// </summary>
        AdminRequest = 1,

        /// <summary>
        /// 前台请求
        /// </summary>
        AppRequest = 2,

        /// <summary>
        /// 后台异常
        /// </summary>
        AdminException = 3,

        /// <summary>
        /// 前台异常
        /// </summary>
        AppException = 4,

        /// <summary>
        /// 后台非法请求   
        /// </summary>
        AdminBadRequest = 5,

        /// <summary>
        /// 前台非法请求
        /// </summary>
        AppBadRequest = 6
    }

    /// <summary>
    /// 操作日志类型
    /// </summary>
    public enum OperateLogType
    {
        None = -1,

        /// <summary>
        /// 登录
        /// </summary>
        Login = 1,
    }
}
