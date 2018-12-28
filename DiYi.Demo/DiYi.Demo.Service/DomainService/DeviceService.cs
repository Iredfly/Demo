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
        /// <summary>
        ///  0,成功，
        ///  -1失败
        ///  >0 已绑定
        /// </summary>
        /// <param name="deviceIn"></param>
        /// <returns></returns>
        public int bind(BindDeviceInDto deviceIn)
        {

            string sql = "Select count(1) From user_device WHERE DeviceNo=@DeviceNo AND IsDeleted=0 ";
            int count = ExecuteScalar<int>(sql, new { deviceIn.DeviceNo });
            if (count > 0)
            {
                return count;
            }

            sql = "Select count(1) From user_device WHERE UserId=@UserId AND DeviceNo=@DeviceNo AND IsDeleted=0";
            count = ExecuteScalar<int>(sql, new { deviceIn.UserId, deviceIn.DeviceNo });
            if (count == 0)
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
                return Add<WxUserDevice>(userDevice) ? 0 : -1;
            }
            else
            {
                return 1;
            }
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

        /// <summary>
        /// 获取用户的设备
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<DeviceOutDto> GetDevices(int UserId)
        {
            string sql = @"SELECT ud.* FROM user_device ud left JOIN
user_extend ue on ue.UserId = ud.UserId AND ue.IsDeleted = 0
WHERE ud.UserId =@UserId  AND ud.IsDeleted = 0 AND ue.UserType = 1";
            return QueryList<DeviceOutDto>(sql, new { UserId });
        }
    }
}
