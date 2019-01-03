using DiYi.Demo.EntityDto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DiYi.Demo.Service;
using DiYi.Demo.EntityDto;
using System.Configuration;
using System.Net.Sockets;
using Newtonsoft.Json;
using DiYi.Demo.Common;
using DiYi.Msg.Socket;
using DiYi.Demo.Api.Models;
using System.IO;
using System.Text;

namespace DiYi.Demo.Api.Controllers
{
    /// <summary>
    /// 设备
    /// </summary>
    public class DeviceController : BaseController
    {
        DeviceService deviceService = new DeviceService();
        public UserService userService = new UserService();
        /// <summary>
        /// 绑定设备
        /// </summary>
        /// <param name="sendCommandIn"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<bool> Bind(BindDeviceInDto sendCommandIn)
        {
            OutDto<bool> ret = new OutDto<bool>();
            if (string.IsNullOrEmpty(sendCommandIn.DeviceNo))
            {
                ret.Data = false;
                ret.Message = "绑定失败，缺少设备编号";
                ret.Code = (int)ResponseCode.Fail;
            }
            int result = deviceService.bind(sendCommandIn);
            if (result == 0)
            {
                ret.Data = true;
                ret.Message = "绑定成功";
                ret.Code = (int)ResponseCode.Success;
            }
            else if (result < 0)
            {
                ret.Data = false;
                ret.Message = "绑定失败";
                ret.Code = (int)ResponseCode.Error;
            }
            else if (result > 0)
            {
                ret.Data = false;
                ret.Message = "该设备已被绑定";
                ret.Code = (int)ResponseCode.Error;
            }
            return ret;
        }
        /// <summary>
        /// 解绑设备
        /// </summary>
        /// <param name="sendCommandIn"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<bool> UnBind(DeviceInDto sendCommandIn)
        {
            OutDto<bool> ret = new OutDto<bool>();
            ret.Data = deviceService.Unbind(sendCommandIn);
            ret.Code = (int)ResponseCode.Success;
            return ret;
        }
        /// <summary>
        /// 开箱
        /// </summary>
        /// <param name="openDeviceIn"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<bool> Open(OpenDeviceInDto openDeviceIn)
        {
            OutDto<bool> outDto = new OutDto<bool>();
            try
            {
                // var list = userService.GetUserDevice(openDeviceIn.UserId, openDeviceIn.DeviceNo);
                var info = deviceService.GetDevice(openDeviceIn.DeviceNo);
                if (info == null)
                {
                    outDto.Data = false;
                    outDto.Message = "未能找到设备";
                    outDto.Code = (int)ResponseCode.Error;
                    return outDto;
                }
                var user = userService.GetUserExtendUserType(info.UserId);
                if (user == null)
                {
                    outDto.Data = false;
                    outDto.Message = "未能找到盒子主人";
                    outDto.Code = (int)ResponseCode.Error;
                    return outDto;

                }

                if (openDeviceIn.UserType == 2)
                {
                    var courier = userService.IsCourier(openDeviceIn.UserId);
                    if (courier)
                    {
                        outDto.Data = opendevice(openDeviceIn.DeviceNo, user.Mobile, openDeviceIn.UserId);
                        outDto.Code = (int)ResponseCode.Success;
                    }
                    else
                    {
                        outDto.Data = false;
                        outDto.Message = "对应用户类型不正确";
                        outDto.Code = (int)ResponseCode.Error;
                    }
                }
                else
                {
                    if (user.UserId == openDeviceIn.UserId)
                    {
                        outDto.Data = opendevice(openDeviceIn.DeviceNo, user.Mobile, openDeviceIn.UserId);
                        outDto.Code = (int)ResponseCode.Success;
                    }
                    else
                    {
                        outDto.Data = false;
                        outDto.Message = "用户错误";
                        outDto.Code = (int)ResponseCode.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return outDto;

        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="openDeviceIn"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<bool> Set(BindDeviceInDto sendCommandIn)
        {
            OutDto<bool> ret = new OutDto<bool>();

            if (string.IsNullOrEmpty(sendCommandIn.DeviceNo))
            {
                ret.Data = false;
                ret.Message = "缺失失败编号";
                ret.Code = (int)ResponseCode.Fail;
                return ret;
            }

            if (string.IsNullOrEmpty(sendCommandIn.DevicePwd))
            {
                ret.Data = false;
                ret.Message = "缺失设备密码";
                ret.Code = (int)ResponseCode.Fail;
            }
            bool result = deviceService.Set(sendCommandIn);
            if (result)
            {
                ret.Data = true;
                ret.Message = "设置成功";
                ret.Code = (int)ResponseCode.Success;
            }
            else
            {
                ret.Data = false;
                ret.Message = "设置失败";
                ret.Code = (int)ResponseCode.Error;
            }
            return ret;
        }
        /// <summary>
        /// 获取用户的设备
        /// </summary>
        /// <param name="deviceDto"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<List<DeviceOutDto>> List(UserDeviceDto deviceDto)
        {
            OutDto<List<DeviceOutDto>> outDto = new OutDto<List<DeviceOutDto>>();

            var data = deviceService.GetDevices(deviceDto.UserId);
            if (data != null)
            {
                outDto.Data = data;
                outDto.Message = "获取成功";
                outDto.Code = (int)ResponseCode.Success;
            }
            else
            {
                outDto.Code = (int)ResponseCode.Error;
                outDto.Message = "获取失败";
            }
            return outDto;
        }
        /// <summary>
        ///  获取设备信息
        /// </summary>
        /// <param name="deviceIn"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<DeviceOutDto> Detail(DeviceInDto deviceIn)
        {
            OutDto<DeviceOutDto> outDto = new OutDto<DeviceOutDto>();
            var data = deviceService.GetDevice(deviceIn.DeviceNo);
            if (data != null)
            {
                outDto.Data = new DeviceOutDto()
                {
                    Area = data.Area,
                    City = data.City,
                    DeviceName = data.DeviceName,
                    Province = data.Province
                };
                outDto.Message = "获取成功";
                outDto.Code = (int)ResponseCode.Success;
            }
            else
            {
                outDto.Code = (int)ResponseCode.Error;
                outDto.Message = "获取失败";
            }
            return outDto;

        }

        #region  通讯

        public static string SServerHost = ConfigurationManager.AppSettings["SServerHost"] ?? "";
        public static int SServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["SServerPort"]);
        private bool opendevice(string DeviceNo, string UserMobile, int userid)
        {
            bool received = false;
            using (Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                client.ReceiveTimeout = 3000;
                client.SendTimeout = 3000;

                if (!client.Connected)
                {
                    try
                    {
                        client.Connect(SServerHost, SServerPort);

                        var cmd = new
                        {
                            Msgid = TimeHelper.GetTimeStamp(DateTime.Now, 10),
                            Method = "2",
                            AccountName = UserMobile,
                            DeviceSn = DeviceNo,
                            DeviceType = "4",
                            ClientSn = userid.ToString()
                        };
                        var sendData = GetSocketRequestData(JsonConvert.SerializeObject(cmd));//请求数据
                        client.Send(sendData);//发送请求数据
                        var end = DateTime.Now.AddSeconds(5);
                        while (!received)
                        {
                            byte[] recvBytes = new byte[1024 * 1024];
                            int length = client.Receive(recvBytes);
                            if (length >= 0)
                            {
                                string str = Encoding.UTF8.GetString(recvBytes, 0, length);
                                logger.Info("Socket接收：" + str);
                                if (!string.IsNullOrEmpty(str))
                                {
                                    str = str.Replace("boxdemo", ""); //boxdemo
                                    var msg = JsonConvert.DeserializeObject<DiYiMsg>(str);
                                    if (msg.Code == "1" && msg.CellStatus == "1")
                                    {
                                        received = true;
                                        break;
                                    }
                                    break;
                                }
                            }
                            if (received == false && DateTime.Now > end)
                            {
                                received = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        client.Shutdown(SocketShutdown.Both);
                        client.Close();
                    }
                }

                //  client.Dispose();
            }
            return received;
        }


        private byte[] GetSocketRequestData(string content)
        {
            content = "boxdemo" + content;
            content += "\r\n";
            logger.Info("Socket发送：" + content);
            byte[] bytes = Encoding.GetEncoding("gbk").GetBytes(content.ToCharArray(), 0, content.Length);
            return bytes;
        }

        #endregion
    }
}
