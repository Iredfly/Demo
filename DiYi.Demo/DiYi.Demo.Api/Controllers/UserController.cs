using DiYi.Demo.EntityDto;
using DiYi.Demo.EntityDto.DBEntity;
using DiYi.Demo.EntityDto.Dto;
using DiYi.Demo.Service;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;
using System;

namespace DiYi.Demo.Api.Controllers
{
    /// <summary>
    /// 用户信息相关
    /// </summary>
    public class UserController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        public UserService userService = new UserService();
        /// <summary>
        /// 
        /// </summary>
        public SendMsgService sendMsgService = new SendMsgService();
        /// <summary>
        /// 用户授权后-获取用户信息
        /// </summary>
        /// <param name="wxUserInfoIn"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<WxUserInfoOutDto> WxUser(WxUserInfoInDto wxUserInfoIn)
        {
            OutDto<WxUserInfoOutDto> wxuserinfo = new OutDto<WxUserInfoOutDto>();
            string UserOpenId = string.Empty;

            string appid = WechatAppId;
            string ComponentToken = GetComponentToken(WxBaseAppId);
            if (!string.IsNullOrEmpty(wxUserInfoIn.Code))
            {
                UserAccessToken accesstoken = WeChatService.GetAccessToken(wxUserInfoIn.Code, appid, ComponentToken, WxBaseAppId);

                if (accesstoken != null && !string.IsNullOrEmpty(accesstoken.openid))
                {
                    UserOpenId = accesstoken.openid;
                }
            }
            else
            {
                UserOpenId = wxUserInfoIn.OpenId;
            }

            var clientuser = new ClientUser();

            //必须根据用户openid获取用户信息，才能知道是否关注公众号
            if (!string.IsNullOrEmpty(UserOpenId))
            {
                string apptoken = AuthorizerAToken(appid);
                var wxuser = WeChatService.GetUserInfo(apptoken, UserOpenId);
                if (wxuser != null && !string.IsNullOrEmpty(wxuser.openid))
                {
                    var user = userService.GetUser(wxuser.openid);
                    if (user == null)
                    {
                        int UserId = userService.AddUser(wxuser);
                        clientuser.UserId = UserId;
                    }
                    else
                    {
                        clientuser.UserId = user.Id;
                        var list = userService.GetUserExtend(clientuser.UserId);
                        if (list != null && list.Count > 0)
                        {
                            if (list.Count == 1)
                            {
                                if (list[0].UserType == 1)
                                {
                                    clientuser.IsMaster = true;
                                }
                                if (list[0].UserType == 2)
                                {
                                    clientuser.IsCourier = true;
                                }
                            }
                            else
                            {
                                clientuser.IsMaster = true;
                                clientuser.IsCourier = true;
                            }
                        }
                    }
                    clientuser.OpenId = wxuser.openid;
                    clientuser.Nickname = wxuser.nickname;
                    clientuser.HeadImgUrl = wxuser.headimgurl;

                    wxuserinfo.Data = clientuser;
                    wxuserinfo.Code = (int)ResponseCode.Success;
                    wxuserinfo.Message = "获取成功";
                    return wxuserinfo;
                }
                else
                {
                    wxuserinfo.Code = (int)ResponseCode.Fail;
                    wxuserinfo.Message = "获取用户信息失败";
                    return wxuserinfo;
                }
            }
            else
            {
                wxuserinfo.Code = (int)ResponseCode.ParameterError;
                wxuserinfo.Message = "参数错误";
                return wxuserinfo;
            }

        }



        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public OutDto<bool> Code(CodeInDto para)
        {
            OutDto<bool> res = new OutDto<bool>();


            //if (memberService.IsExistMobile(para.Mobile, para.TenantId, shardId))
            //{
            //    res.Code = (int)ResponseCode.Fail;
            //    res.Data = new APIBool { ExcuteResult = false, ExcuteMsg = "该手机已经被绑定" };
            //    res.IsSuccess = false;
            //    res.Message = "该手机已经被绑定";
            //    return res;
            //}

            //随机数
            Random rad = new Random();
            int num = rad.Next(100000, 1000000);

            int Expiry = 5;

            redisService.Insert("BoxDome:BindMobiles:" + para.Mobile, num, Expiry * 60);

            var sign = "【递易智能】";
            //从缓存中读取短信状态返回商户id
            var businessId = redisService.Get<int>("BoxDome:MsgState:" + para.Mobile);

            string content = "您的验证码是" + num + "，有效期" + Expiry + "分钟，请尽快填写验证码完成验证。";


            if (MsgRepaly(content, sign, para.Mobile, businessId))
            {
                res.Code = (int)ResponseCode.Success;
                res.Data = true;
                res.Message = "操作成功";
            }
            else
            {
                res.Code = (int)ResponseCode.Fail;
                res.Data = false;
                res.Message = "发送失败，请稍后再试";
            }

            return res;
        }


        /// <summary>
        /// 短信重发机制
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sign"></param>
        /// <param name="phone"></param>
        /// <param name="businessId"></param>
        /// <returns></returns>
        private bool MsgRepaly(string content, string sign, string phone, int businessId)
        {
            bool isSuccess = false;
            #region 重发机制

            if (businessId < 2)
            {
                businessId++;
            }

            //发送短信
            var businessState = sendMsgService.SendMsg(content, sign, phone, businessId);

            //发送失败
            if (businessState == 0)
            {
                if (businessId == 1)
                {
                    businessId = businessId + 1;
                }
                else
                {
                    businessId = businessId - 1;
                }

                //发送失败选择另一家发送
                businessState = sendMsgService.SendMsg(content, sign, phone, businessId);

                if (businessState > 0)
                {
                    isSuccess = true; //发送成功
                    //记录状态
                    redisService.Insert("BoxDome:MsgState:" + phone, businessState, 86400);
                }
            }
            else
            {
                isSuccess = true;//发送成功
                //记录状态
                redisService.Insert("BoxDome:MsgState:" + phone, businessState, 86400);
            }

            #endregion
            return isSuccess;
        }


        /// <summary>
        /// 主人 绑定手机号
        /// </summary>
        /// <param name="bindMobileIn"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<bool> MasterBindMobile(MasterMobileInDto bindMobileIn)
        {
            OutDto<bool> wxuserinfo = new OutDto<bool>();

            var num = redisService.Get<int>("BoxDome:BindMobiles:" + bindMobileIn.Mobile);
            if (string.IsNullOrEmpty(bindMobileIn.Code))
            {
                wxuserinfo.Code = (int)ResponseCode.Fail;
                wxuserinfo.Message = "验证码不能为空";
                return wxuserinfo;
            }
            if (Convert.ToInt32(bindMobileIn.Code) != num)
            {
                wxuserinfo.Code = (int)ResponseCode.Fail;
                wxuserinfo.Message = "验证码错误";
                return wxuserinfo;
            }


            wxuserinfo.Data = userService.MasterMobile(bindMobileIn);
            wxuserinfo.Message = "操作成功";
            if (!wxuserinfo.Data)
            {
                wxuserinfo.Message = "操作失败";
            }
            wxuserinfo.Code = (int)ResponseCode.Success;
            return wxuserinfo;
        }


        /// <summary>
        ///  快递员绑定手机号
        /// </summary>
        /// <param name="bindMobileIn"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<bool> BindMobile(BindMobileInDto bindMobileIn)
        {
            OutDto<bool> wxuserinfo = new OutDto<bool>();


            var num = redisService.Get<int>("BoxDome:BindMobiles:" + bindMobileIn.Mobile);
            if (string.IsNullOrEmpty(bindMobileIn.Code))
            {
                wxuserinfo.Code = (int)ResponseCode.Fail;
                wxuserinfo.Message = "验证码不能为空";
                return wxuserinfo;
            }
            if (Convert.ToInt32(bindMobileIn.Code) != num)
            {
                wxuserinfo.Code = (int)ResponseCode.Fail;
                wxuserinfo.Message = "验证码错误";
                return wxuserinfo;
            }

            wxuserinfo.Data = userService.BindMobile(bindMobileIn);
            wxuserinfo.Message = "操作成功";
            if (!wxuserinfo.Data)
            {
                wxuserinfo.Message = "操作失败";
            }
            wxuserinfo.Code = (int)ResponseCode.Success;
            return wxuserinfo;
        }

        /// <summary>
        /// 实名认证
        /// </summary>
        /// <param name="certUserIn"></param>
        /// <returns></returns>
        [HttpPost]
        public OutDto<bool> CertUser(CertUserInDto certUserIn)
        {
            OutDto<bool> wxuserinfo = new OutDto<bool>();

            wxuserinfo.Data = userService.CertUser(certUserIn);
            wxuserinfo.Message = "操作成功";
            if (!wxuserinfo.Data)
            {
                wxuserinfo.Message = "操作失败";
            }
            wxuserinfo.Code = (int)ResponseCode.Success;
            return wxuserinfo;
        }



        ///// <summary>
        ///// 解绑
        ///// </summary>
        ///// <param name="unbindMobileIn"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public OutDto<bool> UnbindMobile(UnbindMobileInDto unbindMobileIn)
        //{
        //    OutDto<bool> wxuserinfo = new OutDto<bool>();

        //    if (string.IsNullOrEmpty(unbindMobileIn.DeviceNo))
        //    {
        //        unbindMobileIn.DeviceNo = DeviceNo;
        //    }
        //    wxuserinfo.Data = userService.UnbindMobile(unbindMobileIn);
        //    wxuserinfo.Message = "操作成功";
        //    if (!wxuserinfo.Data)
        //    {
        //        wxuserinfo.Message = "操作失败";
        //    }
        //    wxuserinfo.Code = (int)ResponseCode.Success;
        //    return wxuserinfo;
        //}


        #region      

        /// <summary>
        /// 获取公司token
        /// </summary>
        /// <param name="ComponentAppId"></param>
        /// <returns></returns>
        protected string GetComponentToken(string ComponentAppId)
        {

            string TokenKey = ComponentAppId + ":CAT";
            TokenCache token = redisService.Get<TokenCache>(TokenKey);
            if (token != null)
            {
                return token.Token;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="AppId"></param>
        /// <returns></returns>
        protected string AuthorizerAToken(string AppId)
        {
            string TokenKey = "AuthorizerAToken:" + AppId;
            TokenCache token = redisService.Get<TokenCache>(TokenKey);
            if (token != null)
            {
                return token.Token;
            }
            return string.Empty;
        }
        #endregion
    }
}
