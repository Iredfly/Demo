using DiYi.Demo.EntityDto.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.DBEntity
{
    /// <summary>
    /// 微信端用户
    /// </summary>
    [DbTable("user")]
    public class WechatUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [DbKeyColumn]
        public int Id { get; set; }
        /// <summary>
        /// 微信公众号OpenId
        /// </summary>
        [DbColumn]
        public string WxOpenId { get; set; }
        /// <summary>
        ///  微信UnionId
        /// </summary>
        [DbColumn]
        public string WxUnionId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [DbColumn]
        public string Nickname { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [DbColumn]
        public string Photo { get; set; }
        /// <summary>
        ///  性别0未知，1男，2女
        /// </summary>
        [DbColumn]
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DbColumn]
        public string Email { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        [DbColumn]
        public string WxNo { get; set; }
        /// <summary>
        /// 是否关注公众号
        /// </summary>
        [DbColumn]
        public int WxSubscribe { get; set; }
        /// <summary>
        /// 微信公众关注时间
        /// </summary>
        [DbColumn]
        public DateTime WxSubscribeTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DbColumn]
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DbColumn]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [DbColumn]
        public bool IsDeleted { get; set; }
    }
}
