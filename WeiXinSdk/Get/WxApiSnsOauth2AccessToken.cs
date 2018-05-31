using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiSnsOauth2AccessToken : IWxApiGetRequest<WxApiSnsOauth2AccessTokenResult>
    {

        private string appid { get; set; }
        private string secret { get; set; }
        private string code { get; set; }
        private string grant_type { get; set; }

        public string ApiName
        {
            get
            {
                return "sns/oauth2/access_token";
            }
        }

        public WxApiSnsOauth2AccessToken(string appid, string secret, string code, string grant_type)
        {
            this.grant_type = grant_type;
            this.appid = appid;
            this.secret = secret;
            this.code = code;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("appid", this.appid);
            parameters.Add("secret", this.secret);
            parameters.Add("code", this.code);
            parameters.Add("grant_type", this.grant_type);
            return parameters;
        }
    }
}
