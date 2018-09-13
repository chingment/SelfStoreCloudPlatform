using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiGetCardApiTicket : IWxApiGetRequest<WxApiGetCardApiTicketResult>
    {
        public string ApiUrl
        {
            get
            {
                return "https://api.weixin.qq.com/cgi-bin/ticket/getticket";
            }
        }

        private string access_token { get; set; }

        public WxApiGetCardApiTicket(string access_token)
        {
            this.access_token = access_token;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("access_token", this.access_token);
            parameters.Add("type", "wx_card");
            return parameters;
        }
    }
}
