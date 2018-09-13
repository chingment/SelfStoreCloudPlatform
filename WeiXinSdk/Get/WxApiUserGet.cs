using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiUserGet : IWxApiGetRequest<WxApiUserGetResult>
    {
        private string access_token { get; set; }

        private string next_openid { get; set; }

        public string ApiUrl
        {
            get
            {
                return "https://api.weixin.qq.com/cgi-bin/user/get";
            }
        }

        public WxApiUserGet(string access_token, string next_openid)
        {
            this.access_token = access_token;
            this.next_openid = next_openid;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("access_token", this.access_token);
            parameters.Add("next_openid", this.next_openid);
            return parameters;
        }
    }
}
