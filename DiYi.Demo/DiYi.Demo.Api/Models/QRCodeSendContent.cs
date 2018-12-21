using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiYi.Demo.Api.Models
{
    /// <summary>
    /// 命令返回内容
    /// </summary>
    public class ResultContent
    {
        /// <summary>
        /// 
        /// </summary>
        public int MsgId { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CellStatus { get; set; }
    }
}