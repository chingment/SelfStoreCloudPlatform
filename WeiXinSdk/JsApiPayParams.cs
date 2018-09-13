using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class JsApiPayParams
    {
        private string _appId = "";
        private string _timestamp = CommonUtil.GetTimeStamp();
        private string _nonceStr = CommonUtil.GetNonceStr();
        private string _package = "";
        private string _signType = "MD5";
        private string _paySign = "";
        private string _prepayId = "";

        //扩展属性
        private string _orderSn = "";
        private string _orderId = "0";
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

        public string package
        {
            get
            {
                return _package;
            }
        }

        public string signType
        {
            get
            {
                return _signType;
            }
        }

        public string paySign
        {
            get
            {
                return _paySign;
            }
        }

        public string OrderSn
        {
            get
            {
                return _orderSn;
            }
        }

        public string OrderId
        {
            get
            {
                return _orderId;
            }
        }

        public string PrepayId
        {
            get
            {
                return _prepayId;
            }
        }

        public JsApiPayParams(string appId, string key, string prepay_id, string orderId, string orderSn)
        {
            _orderSn = orderSn;
            _orderId = orderId;
            _appId = appId;

            SortedDictionary<string, object> sParams = new SortedDictionary<string, object>();

            _package = string.Format("prepay_id={0}", prepay_id);

            sParams.Add("appId", _appId);
            sParams.Add("package", _package);
            sParams.Add("nonceStr", _nonceStr);
            sParams.Add("timeStamp", _timestamp);
            sParams.Add("signType", _signType);

            string sign = CommonUtil.MakeMd5Sign(sParams, key);

            _paySign = sign;

            sParams.Add("paySign", _paySign);

            _prepayId = prepay_id;
        }
    }
}
