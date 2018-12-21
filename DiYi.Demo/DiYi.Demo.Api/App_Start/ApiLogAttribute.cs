using DiYi.Demo.EntityDto;
using DiYi.Demo.EntityDto.Dto;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace DiYi.Demo.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiLogAttribute : ActionFilterAttribute
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count > 0)
            {
                #region 模型验证

                //if (!actionContext.ModelState.IsValid)
                //{
                //    string msg = string.Empty;
                //    foreach (ModelState modelState in actionContext.ModelState.Values)
                //    {
                //        foreach (ModelError error in modelState.Errors)
                //        {
                //            msg += error.ErrorMessage;
                //        }
                //    }

                //    OutDto<object> dto = new OutDto<object>()
                //    {
                //        Code = (int)ResponseCode.ParameterError,
                //        Message = "参数错误。" + msg
                //    };

                //    //  BadRequestLog.WriteLog(actionContext, dto.ToJson());

                //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, dto);
                //    return;
                //}

                #endregion


                //记录入参  actionContext.ActionArguments
                string inParas = "接口入参 # ";

                if (actionContext.ActionArguments != null) inParas += JsonConvert.SerializeObject(actionContext.ActionArguments);
                else
                {
                    //获取请求数据  
                    Stream stream = actionContext.Request.Content.ReadAsStreamAsync().Result;
                    string requestDataStr = "";
                    if (stream != null && stream.Length > 0)
                    {
                        stream.Position = 0; //当你读取完之后必须把stream的读取位置设为开始
                        using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                        {
                            requestDataStr = reader.ReadToEnd().ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(requestDataStr)) inParas += requestDataStr;
                }


                logger.Info(actionContext.Request.RequestUri.LocalPath + " # " + inParas + " # ");

                base.OnActionExecuting(actionContext);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                string inParas = "接口入参 # ";
                if (actionExecutedContext != null && actionExecutedContext.ActionContext != null && actionExecutedContext.ActionContext.ActionArguments != null) inParas += JsonConvert.SerializeObject(actionExecutedContext.ActionContext.ActionArguments);
                else
                {
                    //获取请求数据  
                    Stream stream = actionExecutedContext.ActionContext.Request.Content.ReadAsStreamAsync().Result;
                    string requestDataStr = "";
                    if (stream != null && stream.Length > 0)
                    {
                        stream.Position = 0; //当你读取完之后必须把stream的读取位置设为开始
                        using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                        {
                            requestDataStr = reader.ReadToEnd().ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(requestDataStr)) inParas += requestDataStr;
                }

                if (logger != null)
                {
                    if (actionExecutedContext.Response != null && actionExecutedContext.Response.Content != null)
                    {
                        var res = actionExecutedContext.Response.Content.ReadAsStringAsync();
                        if (res != null)
                        {
                            logger.Info(actionExecutedContext.Request.RequestUri.LocalPath + " # 接口出参:" + actionExecutedContext.Response.Content.ReadAsStringAsync().Result + "# " + inParas + " # ");
                        }

                    }
                    else
                    {
                        logger.Info(actionExecutedContext.Request.RequestUri.LocalPath + " # 接口出参:无" + inParas + " # ");
                    }

                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                logger.Error("输出异常：" + ex.ToString());
            }

            base.OnActionExecuted(actionExecutedContext);
        }


    }


    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute //System.Web.Mvc.IExceptionFilter//
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            logger.Error("接口异常:" + actionExecutedContext.Exception.ToString(), 3);

            //2.返回调用方具体的异常信息
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
            else if (actionExecutedContext.Exception is OperationCanceledException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            //.....这里可以根据项目需要返回到客户端特定的状态码。如果找不到相应的异常，统一返回服务端错误500
            else
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            base.OnException(actionExecutedContext);
        }
    }
}
