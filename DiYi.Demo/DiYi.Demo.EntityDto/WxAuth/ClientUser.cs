using DiYi.Demo.EntityDto.Dto;
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
    public class ClientUser : WxUserInfoOutDto
    {
        ///// <summary>
        ///// 用户类别以及绑定的手机
        ///// </summary>
        //public List<DeviceUserType> TypeList { get; set; }
    }
    /// <summary>
    ///   用户类别以及绑定的手机
    /// </summary>
    public class DeviceUserType
    {
        /// <summary>
        /// 类别 0：未知，1:主人，2：快递员
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }


    }
}
