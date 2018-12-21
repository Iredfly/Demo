using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.Dto
{
    /// <summary>
    /// 绑定手机号
    /// </summary>
    public class MasterMobileInDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }

    public class BindMobileInDto : MasterMobileInDto
    {
        /// <summary>
        /// 快递公司 
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
    }

    public class CertUserInDto
    {
        /// <summary>
        ///    用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNo { get; set; }
        /// <summary>
        /// 正面照
        /// </summary>
        public string Face { get; set; }
        /// <summary>
        /// 反面照
        /// </summary>
        public string Back { get; set; }
        /// <summary>
        /// 手持证件照
        /// </summary>
        public string Hold { get; set; }
    }

    public class CodeInDto
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
    }
    /// <summary>
    /// 解除绑定
    /// </summary>
    public class UnbindMobileInDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户类别 1:盒子主人，2：快递员
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceNo { get; set; }
    }
}
