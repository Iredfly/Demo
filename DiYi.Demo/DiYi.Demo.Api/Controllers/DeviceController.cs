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
            if (string.IsNullOrEmpty(sendCommandIn.DeviceNo))
            {
                sendCommandIn.DeviceNo = DeviceNo;
            }
            OutDto<bool> ret = new OutDto<bool>();
            ret.Data = deviceService.bind(sendCommandIn);
            ret.Code = (int)ResponseCode.Success;
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
            if (string.IsNullOrEmpty(sendCommandIn.DeviceNo))
            {
                sendCommandIn.DeviceNo = DeviceNo;
            }
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
                var list = userService.GetUserDevice(openDeviceIn.UserId, openDeviceIn.DeviceNo);
                var item = list.FirstOrDefault(e => e.UserType == openDeviceIn.UserType);
                if (item != null)
                {
                    outDto.Data = opendevice(openDeviceIn.DeviceNo, item.Mobile, openDeviceIn.UserId);
                    outDto.Code = (int)ResponseCode.Success;
                }
                else
                {
                    outDto.Data = false;
                    outDto.Code = (int)ResponseCode.Error;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
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
            if (string.IsNullOrEmpty(deviceIn.DeviceNo))
            {
                deviceIn.DeviceNo = DeviceNo;
            }
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
                            logger.Info("Sock接收：" + str);
                            if (!string.IsNullOrEmpty(str))
                            {
                                str = str.Replace("boxdemo", ""); //boxdemo
                                var msg = JsonConvert.DeserializeObject<DiYiMsg>(str);
                                if (msg.Code == "1")
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
                    //if (received)
                    //{
                    //    received = false;
                    //    var opencmd = new
                    //    {
                    //        Msgid = TimeHelper.GetTimeStamp(DateTime.Now, 10),
                    //        Method = "2",
                    //        AccountName = UserMobile,
                    //        DeviceSn = DeviceNo,
                    //        DeviceType = "4",
                    //        ClientSn = userid.ToString()
                    //    };
                    //    var opensendData = GetSocketRequestData(JsonConvert.SerializeObject(opencmd));//请求数据
                    //    client.Send(opensendData);//发送请求数据

                    //    while (!received)
                    //    {
                    //        byte[] recvBytes = new byte[1024 * 1024];
                    //        int length = client.Receive(recvBytes);
                    //        if (length >= 0)
                    //        {
                    //            string str = Encoding.UTF8.GetString(recvBytes, 0, length);
                    //            if (!string.IsNullOrEmpty(str))
                    //            {
                    //                logger.Info("Sock接收：" + str);
                    //                if (str.Length > 7)
                    //                {
                    //                    str = str.Substring(7); //boxdemo
                    //                    var msg = JsonConvert.DeserializeObject<DiYiMsg>(str);
                    //                    if (msg.Code == "1" && msg.CellStatus == "1")
                    //                    {
                    //                        received = true;
                    //                    }
                    //                }
                    //                break;
                    //            }
                    //        }
                    //        if (received == false && DateTime.Now > end)
                    //        {
                    //            received = true;
                    //        }
                    //    }
                    //}
                }
            }
            return received;
        }


        private byte[] GetSocketRequestData(string content)
        {
            content = "boxdemo" + content;
            content += "\r\n";
            logger.Info("Sock发送：" + content);
            byte[] bytes = Encoding.GetEncoding("gbk").GetBytes(content.ToCharArray(), 0, content.Length);
            return bytes;
        }




        //private byte[] GetSocketRequestData(string method, string content, string domain = "Post")
        //{
        //    //DiYiMsg msg = new DiYiMsg();
        //    //msg.Method = method;
        //    //msg.Domain = domain;
        //    //msg.Content = content;
        //    var ct = "boxdemo" + content;
        //    logger.Info("Sock发送：" + ct);

        //    // var data = PBSerialize(msg);
        //    var data = PBSerialize(ct);

        //    var sendData = SocketProtocal.Parse(data, SocketProtocalType.Command).ToBytes();
        //    return sendData;
        //}
        /// <summary>  
        /// 截取字节数组  
        /// </summary>  
        /// <param name="srcBytes">要截取的字节数组</param>  
        /// <param name="startIndex">开始截取位置的索引</param>  
        /// <param name="length">要截取的字节长度</param>  
        /// <returns>截取后的字节数组</returns>  
        private byte[] SubByte(byte[] srcBytes, int startIndex, int length)
        {
            System.IO.MemoryStream bufferStream = new System.IO.MemoryStream();
            byte[] returnByte = new byte[] { };
            if (srcBytes == null) { return returnByte; }
            if (startIndex < 0) { startIndex = 0; }
            if (startIndex < srcBytes.Length)
            {
                if (length < 1 || length > srcBytes.Length - startIndex) { length = srcBytes.Length - startIndex; }
                bufferStream.Write(srcBytes, startIndex, length);
                returnByte = bufferStream.ToArray();
                bufferStream.SetLength(0);
                bufferStream.Position = 0;
            }
            bufferStream.Close();
            bufferStream.Dispose();
            return returnByte;
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private byte[] PBSerialize<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize<T>(ms, t);
                byte[] result = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(result, 0, result.Length);
                return result;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static T PBDeserialize<T>(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                return (T)ProtoBuf.Serializer.Deserialize<T>(ms);
            }
        }

        #endregion
    }
}
