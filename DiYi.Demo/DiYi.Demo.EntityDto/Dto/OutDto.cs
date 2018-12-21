using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class OutDto<T>
    {
        /// <summary>
        /// 返回状态码 
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 状态码的描述信息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 服务器返回时间
        /// </summary>
        public string RespTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        /// <summary>
        /// 具体业务数据
        /// </summary>
        public T Data { get; set; }
    }

}
