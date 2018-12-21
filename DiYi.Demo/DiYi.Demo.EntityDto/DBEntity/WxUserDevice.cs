using DiYi.Demo.EntityDto.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.EntityDto.DBEntity
{
    /// <summary>
    /// 用户绑定设备
    /// </summary>
    [DbTable("user_device")]
    public class WxUserDevice
    {
        [DbKeyColumn]
        public int Id { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        [DbColumn]
        public string DeviceNo { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [DbColumn]
        public int UserId { get; set; }

        [DbColumn]
        public string DeviceName { get; set; }
        [DbColumn]
        public string DevicePwd { get; set; }
        [DbColumn]
        public int ProvinceId { get; set; }
        [DbColumn]
        public int CityId { get; set; }
        [DbColumn]
        public int AreaId { get; set; }
        [DbColumn]
        public string  Province { get; set; }
        [DbColumn]
        public string City { get; set; }
        [DbColumn]
        public string Area { get; set; }
        [DbColumn]
        public string Detail { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DbColumn]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DbColumn]
        public bool IsDeleted { get; set; }

    }
}
