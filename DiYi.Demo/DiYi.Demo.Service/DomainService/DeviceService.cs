using DiYi.Demo.EntityDto.DBEntity;
using DiYi.Demo.EntityDto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.Service
{
    public class DeviceService : MySqlService
    {
        public bool bind(BindDeviceInDto deviceIn)
        {
            WxUserDevice userDevice = new WxUserDevice()
            {
                UserId = deviceIn.UserId,
                AreaId = deviceIn.AreaId,
                CityId = deviceIn.CityId,
                CreateTime = DateTime.Now,
                Detail = deviceIn.Detail,
                DeviceName = deviceIn.DeviceName,
                DeviceNo = deviceIn.DeviceNo,
                DevicePwd = deviceIn.DevciePwd,
                ProvinceId = deviceIn.ProvinceId,
                Province = deviceIn.Province,
                Area = deviceIn.Area,
                City = deviceIn.City,
                IsDeleted = false
            };
            return Add<WxUserDevice>(userDevice);
        }

        public bool Unbind(DeviceInDto deviceIn)
        {
            string sql = "Update user_device set IsDeleted=1 WHERE UserId=@UserId AND DeviceNo=@DeviceNo ";
            return Execute(sql, deviceIn);
        }

        public WxUserDevice GetDevice(string DeviceNo)
        {
            string sql = "select * from user_device WHERE DeviceNo=@DeviceNo AND IsDeleted=0 ORDER BY CreateTime desc LIMIT 1 ";
            return QuerySingle<WxUserDevice>(sql, new { DeviceNo });
        }
    }
}
