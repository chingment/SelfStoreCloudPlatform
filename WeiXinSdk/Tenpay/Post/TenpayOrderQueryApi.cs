using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.Tenpay
{
    public class TenpayOrderQueryApi : TenpayBasePostApi, ITenpayPostApi
    {
        private string _postData;

        public string ApiName
        {
            get
            {
                return "pay/orderquery";
            }
        }

        public string PostData
        {
            get
            {
                return _postData;
            }
            set
            {
                _postData = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="out_trade_no">商户内部的订单号</param>
        public TenpayOrderQueryApi(IWxConfig config, string out_trade_no)
        {
            SortedDictionary<string, object> sParams = new SortedDictionary<string, object>();

            sParams.Add("appid", config.AppId);//公众账号ID
            sParams.Add("mch_id", config.MchId);//商户号
            sParams.Add("nonce_str", CommonUtil.GetNonceStr());//随机字符串
            sParams.Add("out_trade_no", out_trade_no);//微信订单号

            string sign = MakeSign(sParams, config.Key);
            sParams.Add("sign", sign);//签名

            _postData = GetXml(sParams);

        }
    }
}
