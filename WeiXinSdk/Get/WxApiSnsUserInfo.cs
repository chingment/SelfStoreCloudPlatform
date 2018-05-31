using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiSnsUserInfo : IWxApiGetRequest<WxApiSnsUserInfoResult>
    {
        private string openid { get; set; }
        private string access_token { get; set; }
        private string lang { get; set; }


        public string ApiName
        {
            get
            {
                return "sns/userinfo";
            }
        }

        public WxApiSnsUserInfo(string access_token, string openid, string lang)
        {
            this.openid = openid;
            this.access_token = access_token;
            this.lang = lang;

        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("access_token", this.access_token);
            parameters.Add("openid", this.openid);
            parameters.Add("lang", this.lang);
            return parameters;
        }
    }
}
