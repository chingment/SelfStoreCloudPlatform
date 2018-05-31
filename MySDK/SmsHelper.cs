using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySDK
{
    public class SmsHelper
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="template">模版ID</param>
        /// <param name="smsparam">模版参数名称及参数值（Json格式）</param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static CustomJsonResult Send(string template, string smsparam, string mobile, out string token,string validCode=null, int? expireSecond = null)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = AliyunSingleSendSmsUtils.Send(template, smsparam, mobile, out token, validCode, expireSecond);

            return result;

        }
    }
}
