using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.Dto
{
    /// <summary>
    ///   获取登陆者信息
    /// </summary>
    public class WxUserInfoInDto
    {
        /// <summary>
        /// 用户code
        /// </summary>
        public string Code { get; set; }

        public string OpenId { get; set; }

    }
}
