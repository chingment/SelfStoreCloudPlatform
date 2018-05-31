using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiUserInfo : IWxApiGetRequest<WxApiUserInfoResult>
    {
        private string access_token { get; set; }

        private string openid { get; set; }

        public string ApiName
        {
            get
            {
                return "cgi-bin/user/info";
            }
        }

        public WxApiUserInfo(string access_token, string openid)
        {
            this.access_token = access_token;
            this.openid = openid;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("access_token", this.access_token);
            parameters.Add("openid", this.openid);
            return parameters;
        }
    }
}
