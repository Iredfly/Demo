using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto
{
    public class TokenCache
    {
        /// <summary>
        /// token值
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }
}
