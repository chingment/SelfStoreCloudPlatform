using Lumos.Common;
using Lumos.Redis;
using MySDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class SmsProvider : BaseProvider
    {
        private string key_ReplenishStaffBindMobile = "ReplenishStaffBindMobile:{0}";
        public string BuildValidCode()
        {
            VerifyCodeHelper v = new VerifyCodeHelper();
            v.CodeSerial = "0,1,2,3,4,5,6,7,8,9";
            v.Length = 6;
            string code = v.CreateVerifyCode(); //取随机码 

            return code;
        }

        public CustomJsonResult ReplenishStaffBindMobile(string operater, string phone)
        {
            string validcode = BuildValidCode();
            int seconds = 120;
            string token = "";
            CustomJsonResult result = SmsHelper.Send("SMS_88990017", "{\"code\":\"" + validcode + "\"}", phone, out token, validcode, seconds);
            if (result.Result == ResultType.Success)
            {
                var redis = new RedisClient<string>();
                var key = String.Format(key_ReplenishStaffBindMobile, phone);
                redis.KSet(key, validcode, new TimeSpan(0, 2, 0));
            }
            return result;
        }

        public bool ReplenishStaffBindMobileCheckValidCode(string phone, string validCode)
        {
            var redis = new RedisClient<string>();
            var key = String.Format(key_ReplenishStaffBindMobile, phone);
            string value = redis.KGet(key);

            if (value == null)
                return false;

            if (value != validCode)
                return false;

            return true;
        }
    }
}
