using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiJsCode2Session : IWxApiGetRequest<WxApiJsCode2SessionResult>
    {
        private string appid { get; set; }
        private string secret { get; set; }
        private string code { get; set; }

        public string ApiUrl
        {
            get
            {
                return "https://api.weixin.qq.com/sns/jscode2session";
            }
        }

        public WxApiJsCode2Session(string appid, string secret, string code)
        {
            this.appid = appid;
            this.secret = secret;
            this.code = code;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("appid", this.appid);
            parameters.Add("secret", this.secret);
            parameters.Add("js_code", this.code);
            parameters.Add("grant_type", "authorization_code");
            return parameters;
        }
    }
}
