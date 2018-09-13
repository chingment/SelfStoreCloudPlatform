using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class JsApiConfigParams
    {
        private bool _debug = false;
        private string _appId = "";
        private string _timestamp = CommonUtil.GetTimeStamp();
        private string _nonceStr = CommonUtil.GetNonceStr();
        private string _signature = "";


        public bool debug
        {

            get
            {
                return _debug;
            }
        }
        public string appId
        {
            get
            {
                return _appId;
            }
        }
        public string timestamp
        {
            get
            {
                return _timestamp;
            }
        }
        public string nonceStr
        {
            get
            {
                return _nonceStr;
            }
        }
        public string signature
        {
            get
            {
                return _signature;
            }
        }

        public JsApiConfigParams()
        {

        }


        public JsApiConfigParams(string appId,string url, string ticket)
        {
            _appId = appId;

            SortedDictionary<string, string> sParams = new SortedDictionary<string, string>();
          
            sParams.Add("jsapi_ticket", ticket);
            sParams.Add("noncestr", _nonceStr);
            sParams.Add("timestamp", _timestamp);
            sParams.Add("url", url);

            string sign = CommonUtil.MakeSHA1Sign(sParams);

            _signature = sign;

            sParams.Add("signature", _signature);


        }
    }
}
