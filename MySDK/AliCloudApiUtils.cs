using log4net;
using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MySDK
{
    public class AliCloudApiResult
    {

        public bool Success { get; set; }

        public string Message { get; set; }

    }

    public class AliCloudApiUtils
    {
        private const String host = "http://sms.market.alicloudapi.com";
        private const String path = "/singleSendSms";
        private const String method = "GET";
        private const String appcode = "2805d6a2e9604b21a2e8dca3309ee9fd";

        private static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }




        public static CustomJsonResult Send(string template, string smsparam, string mobile)
        {
            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            LumosDbContext currentDb = new LumosDbContext();

            SysSmsSendHistory sendHistory = new SysSmsSendHistory();
            sendHistory.ApiName = "sms.market.alicloudapi.com";
            sendHistory.TemplateParams = smsparam;
            sendHistory.TemplateCode = template;
            sendHistory.Phone = mobile;
            sendHistory.Creator = 0;
            sendHistory.CreateTime = DateTime.Now;

            CustomJsonResult result = new CustomJsonResult();


            try
            {


                String querys = string.Format("ParamString={1}&RecNum={2}&SignName={3}&TemplateCode={0}", template, UrlEncode(smsparam), mobile, "收款易");

                String bodys = "";
                String url = host + path;
                HttpWebRequest httpRequest = null;
                HttpWebResponse httpResponse = null;

                if (0 < querys.Length)
                {
                    url = url + "?" + querys;
                }

                if (host.Contains("https://"))
                {

                    httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                }
                else
                {
                    httpRequest = (HttpWebRequest)WebRequest.Create(url);
                }
                httpRequest.Method = method;
                httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
                if (0 < bodys.Length)
                {
                    byte[] data = Encoding.UTF8.GetBytes(bodys);
                    using (Stream stream = httpRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));

                string r = reader.ReadToEnd();


                AliCloudApiResult apiResult = Newtonsoft.Json.JsonConvert.DeserializeObject<AliCloudApiResult>(r);

                if (apiResult.Success)
                {

                    sendHistory.Result = Enumeration.SysSmsSendResult.Success;
                    currentDb.SysSmsSendHistory.Add(sendHistory);
                    currentDb.SaveChanges();


                    result = new CustomJsonResult(ResultType.Success, "发送成功");
                }
                else
                {
                    sendHistory.Result = Enumeration.SysSmsSendResult.Failure;
                    sendHistory.FailureReason = string.Format("描述:{0}", apiResult.Message);
                    currentDb.SysSmsSendHistory.Add(sendHistory);
                    currentDb.SaveChanges();

                    log.ErrorFormat("调用短信{0}接口-错误信息:{1}", sendHistory.ApiName, apiResult.Message);

                    result = new CustomJsonResult(ResultType.Failure, "发送失败");
                }


                return result;
            }
            catch (Exception ex)
            {
                sendHistory.Result = Enumeration.SysSmsSendResult.Exception;

                sendHistory.FailureReason = ex.Message;

                currentDb.SysSmsSendHistory.Add(sendHistory);
                currentDb.SaveChanges();

                log.ErrorFormat("调用短信{0}接口-错误信息:{1},描述:{2}", sendHistory.ApiName, ex.Message, ex.StackTrace);

                return new CustomJsonResult(ResultType.Failure, "发送失败");
            }

            return result;
        }



        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
