using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.Dto
{
    public class BindDeviceInDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 家主密码
        /// </summary>
        public string DevciePwd { get; set; }
        /// <summary>
        /// 省Id
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 市Id
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 门牌号
        /// </summary>
        public string Detail { get; set; }
    }

    public class DeviceInDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceNo { get; set; }
    }


    public class OpenDeviceInDto
    {
        /// <summary>
        ///   用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户类别
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceNo { get; set; }
    }

    public class DeviceOutDto
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
    }
}
