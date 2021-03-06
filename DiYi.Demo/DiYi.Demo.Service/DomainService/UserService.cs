﻿using DiYi.Demo.EntityDto.DBEntity;
using DiYi.Demo.EntityDto.Dto;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.Service
{
    public class UserService : MySqlService
    {
        #region 微信用户
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="ShopId"></param>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        public WechatUser GetUser(string OpenId)
        {
            string sql = "SELECT * FROM user WHERE IsDeleted=0 AND  WxOpenId=@OpenId";
            return QuerySingle<WechatUser>(sql, new { OpenId });
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="ShopId"></param>
        /// <param name="weixinUserInfo"></param>
        /// <returns></returns>
        public int AddUser(WeixinUserInfoResult weixinUserInfo)
        {
            if (weixinUserInfo.errcode == 0)
            {

                WechatUser user = new WechatUser()
                {
                    Sex = weixinUserInfo.sex,
                    WxOpenId = weixinUserInfo.openid,
                    Photo = weixinUserInfo.headimgurl,
                    Nickname = weixinUserInfo.nickname,
                    WxSubscribe = weixinUserInfo.subscribe,
                    WxSubscribeTime = ConvertStringToDateTime(weixinUserInfo.subscribe_time),
                    WxUnionId = weixinUserInfo.unionid,

                    CreateTime = DateTime.Now,
                    IsDeleted = false,

                };
                return AddReturnId<WechatUser>(user);
            }
            else
            {
                throw new Exception(weixinUserInfo.errmsg);
            }
        }


        public static DateTime ConvertStringToDateTime(long curSeconds)
        {
            //lTime *= 10000;
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //TimeSpan toNow = new TimeSpan(lTime);
            //return dtStart.Add(toNow);
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(curSeconds);
        }
        #endregion


        #region    设备绑定

        /// <summary>
        /// 绑定手机号
        /// </summary>
        /// <param name="bindMobileIn"></param>
        /// <returns></returns>

        public bool MasterMobile(MasterMobileInDto bindMobileIn)
        {
            string sql = "SELECT * FROM user_extend  WHERE UserId=@UserId AND UserType=@UserType AND Mobile=@Mobile AND IsDeleted=0";
            var userdevice = QuerySingle<WxUserExtend>(sql, new { bindMobileIn.UserId, bindMobileIn.Mobile, UserType = 1 });
            if (userdevice == null)
            {
                WxUserExtend extend = new WxUserExtend()
                {
                    UserType = 1,
                    Mobile = bindMobileIn.Mobile,
                    UserId = bindMobileIn.UserId,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                };
                return Add<WxUserExtend>(extend);
            }
            //else
            //{
            //    string update = "Update user_device Set Mobile=@Mobile,UserType=@UserType  WHERE  UserId=@UserId AND UserType=@UserType AND DeviceNo=@DeviceNo AND IsDeleted=0";
            //    return Execute(update, bindMobileIn);
            //}
            return false;
        }

        public bool BindMobile(BindMobileInDto bindMobileIn)
        {
            string sql = "SELECT * FROM user_extend  WHERE UserId=@UserId AND UserType=@UserType AND Mobile=@Mobile AND IsDeleted=0";
            var userdevice = QuerySingle<WxUserExtend>(sql, new { bindMobileIn.UserId, bindMobileIn.Mobile, UserType = 2 });
            if (userdevice == null)
            {
                WxUserExtend extend = new WxUserExtend()
                {
                    UserType = 2,
                    Mobile = bindMobileIn.Mobile,
                    UserId = bindMobileIn.UserId,
                    Pwd = bindMobileIn.Pwd,
                    Company = bindMobileIn.Company,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                };
                return Add<WxUserExtend>(extend);
            }
            //else
            //{
            //    string update = "Update user_device Set Mobile=@Mobile,UserType=@UserType  WHERE  UserId=@UserId AND UserType=@UserType AND DeviceNo=@DeviceNo AND IsDeleted=0";
            //    return Execute(update, bindMobileIn);
            //}
            return false;
        }

        /// <summary>
        /// 实名认证
        /// </summary>
        /// <param name="certUserIn"></param>
        /// <returns></returns>
        public bool CertUser(CertUserInDto certUserIn)
        {
            string sql = "Update user_extend Set RealName=@RealName,IDNo=@IDNo,Face=@Face,Back=@Back,Hold=@Hold,Status=1  WHERE UserId=@UserId AND UserType=@UserType  AND IsDeleted=0";
            return Execute(sql, new
            {
                certUserIn.Back,
                certUserIn.Face,
                certUserIn.Hold,
                certUserIn.IDNo,
                certUserIn.RealName,
                certUserIn.UserId,
                UserType = 2
            });
        }
        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unbindMobileIn"></param>
        /// <returns></returns>
        public bool UnbindMobile(UnbindMobileInDto unbindMobileIn)
        {
            string sql = "Update user_device set IsDeleted=1  WHERE UserId=@UserId AND DeviceNo=@DeviceNo AND UserType=@UserType AND IsDeleted=0";
            return Execute(sql, unbindMobileIn);
        }
        /// <summary>
        /// 获取用户绑定信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="DeviceNo"></param>
        /// <returns></returns>
        public List<WxUserExtend> GetUserExtend(int UserId)
        {
            string sql = "SELECT * FROM user_extend  WHERE UserId=@UserId  AND IsDeleted=0";
            return QueryList<WxUserExtend>(sql, new { UserId });
        }
        public WxUserExtend GetUserExtendUserType(int UserId)
        {
            string sql = "SELECT * FROM user_extend  WHERE UserId=@UserId  AND IsDeleted=0 AND UserType=1 limit 1";
            return QuerySingle<WxUserExtend>(sql, new { UserId });
        }
        /// <summary>
        /// 获取用户绑定信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="DeviceNo"></param>
        /// <returns></returns>
        public List<OpenDeviceDto> GetUserDevice(int UserId, string DeviceNo)
        {
            string sql = @"SELECT ue.UserType,ue.Mobile FROM user_extend ue left JOIN
user_device ud on ue.UserId = ud.UserId AND ud.IsDeleted = 0
WHERE ue.UserId =@UserId  AND ue.IsDeleted = 0 AND ud.DeviceNo=@DeviceNo";
            return QueryList<OpenDeviceDto>(sql, new { UserId, DeviceNo });
        }
        /// <summary>
        /// 是否是快递员
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool IsCourier(int UserId)
        {
            string sql = @"SELECT count(1) FROM user_extend  WHERE UserId=@UserId AND UserType=2 AND IsDeleted=0";
            return QuerySingle<int>(sql, new { UserId }) > 0;
        }
        #endregion

    }
}
