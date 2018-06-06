using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Lumos;
using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySDK
{
    public class AliyunSingleSendSmsUtils
    {
        public static CustomJsonResult Send(string template, string smsparam, string mobile, out string token, string validCode = null, int? expireSecond = null)
        {
            String product = "Dysmsapi";//短信API产品名称
            String domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            CustomJsonResult result = new CustomJsonResult();
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "LTAI1kaGcK7uE9Hf", "95x0VXSdph8lMvjLRvsv8sscCpTvWL");

            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();

            LumosDbContext currentDb = new LumosDbContext();

            SysSmsSendHistory sendHistory = new SysSmsSendHistory();
            token = Guid.NewGuid().ToString();
            sendHistory.Token = token;
            sendHistory.ApiName = "AliyunSingleSendSmsUtils";
            sendHistory.TemplateParams = smsparam;
            sendHistory.TemplateCode = template;
            sendHistory.Phone = mobile;
            sendHistory.CreateTime = DateTime.Now;
            sendHistory.Creator = "0";
            sendHistory.ValidCode = validCode;
            if (expireSecond != null)
            {
                sendHistory.ExpireTime = sendHistory.CreateTime.AddSeconds(expireSecond.Value);
            }

            try
            {

                request.SignName = "贩聚社团";//"管理控制台中配置的短信签名（状态必须是验证通过）"
                request.PhoneNumbers = mobile;//"接收号码，多个号码可以逗号分隔"
                request.TemplateCode = template;//管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）"
                request.TemplateParam = smsparam;//短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。"
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                if (sendSmsResponse.Code == "OK")
                {
                    sendHistory.Result = Enumeration.SysSmsSendResult.Success;
                    result = new CustomJsonResult(ResultType.Success, "发送成功.");
                }
                else
                {
                    sendHistory.Result = Enumeration.SysSmsSendResult.Failure;
                    sendHistory.FailureReason = sendSmsResponse.Message;
                    result = new CustomJsonResult(ResultType.Failure, "发送失败.");
                }

            }
            catch (ServerException ex)
            {
                sendHistory.Result = Enumeration.SysSmsSendResult.Exception;

                sendHistory.FailureReason = ex.ErrorCode;

                result = new CustomJsonResult(ResultType.Exception, "发送失败..");

            }
            catch (ClientException ex)
            {
                sendHistory.Result = Enumeration.SysSmsSendResult.Exception;

                sendHistory.FailureReason = ex.ErrorCode;

                result = new CustomJsonResult(ResultType.Exception, "发送失败...");
            }
            catch (Exception ex)
            {
                sendHistory.Result = Enumeration.SysSmsSendResult.Exception;

                sendHistory.FailureReason = ex.Message;

                result = new CustomJsonResult(ResultType.Exception, "发送失败....");
            }

            currentDb.SysSmsSendHistory.Add(sendHistory);
            currentDb.SaveChanges();

            return result;
        }
    }
}
