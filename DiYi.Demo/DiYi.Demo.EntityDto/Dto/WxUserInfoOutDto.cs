using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.Dto
{
    /// <summary>
    ///    微信获取用户信息返回结果
    /// </summary>
    public class WxUserInfoOutDto
    {
        /// <summary>
        ///    用户的唯一标识
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 系统用户ID
        /// </summary>
        public int UserId { get; set; }

        ///// <summary>
        ///// 0:未知， 1:主人，2：快递员  ,3：盒子主人和快递员
        ///// </summary>
        //public int UserType { get; set; }

        /// <summary>
        /// 是否是快递员
        /// </summary>
        public bool IsCourier { get; set; }
        /// <summary>
        /// 是否是盒子主人
        /// </summary>
        public bool IsMaster { get; set; }

        ///// <summary>
        ///// 手机号
        ///// </summary>
        //public string Mobile { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string SignToken { get; set; }
        ///// <summary>
        ///// SignToken过期时间
        ///// </summary>
        //public DateTime ExpireTime { get; set; }

    }
}
