using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiAccessToken : IWxApiGetRequest<WxApiAccessTokenResult>
    {
        private string grant_type { get; set; }

        private string appid { get; set; }

        private string secret { get; set; }

        public string ApiName
        {
            get
            {
                return "cgi-bin/token";
            }
        }

        public WxApiAccessToken(string grant_type, string appid, string secret)
        {
            this.grant_type = grant_type;
            this.appid = appid;
            this.secret = secret;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", this.grant_type);
            parameters.Add("appid", this.appid);
            parameters.Add("secret", this.secret);
            return parameters;
        }
    }
}
