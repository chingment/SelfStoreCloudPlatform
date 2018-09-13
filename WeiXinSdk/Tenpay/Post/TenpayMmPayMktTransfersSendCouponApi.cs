using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.Tenpay
{
    public class TenpayMmPayMktTransfersSendCouponApi : TenpayBasePostApi, ITenpayPostApi
    {
        private string _postData;

        public string ApiName
        {
            get
            {
                return "mmpaymkttransfers/send_coupon";
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
        public TenpayMmPayMktTransfersSendCouponApi(IWxConfig config, string coupon_stock_id, string partner_trade_no, string openid)
        {
            SortedDictionary<string, object> sParams = new SortedDictionary<string, object>();

            sParams.Add("appid", config.AppId);
            sParams.Add("coupon_stock_id", coupon_stock_id);
            sParams.Add("mch_id", config.MchId);
            sParams.Add("nonce_str", CommonUtil.GetNonceStr());
            sParams.Add("openid", openid);
            sParams.Add("openid_count", 1);
            sParams.Add("partner_trade_no", partner_trade_no);
            string sign = MakeSign(sParams, config.Key);
            sParams.Add("sign", sign);

             _postData = GetXml(sParams);

           // _postData = "<xml><appid><![CDATA[wxc6e80f8c575cf3f5]]></appid><coupon_stock_id><![CDATA[3068001]]></coupon_stock_id><mch_id><![CDATA[1486589902]]></mch_id><nonce_str><![CDATA[bdb70cd1606a4669a15d5af965811f77]]></nonce_str><openid>otakHv8beYaLD9po9y6WjVe1fqt81</openid><openid_count>1</openid_count><partner_trade_no><![CDATA[202]]></partner_trade_no><sign><![CDATA[76C1EE9C00B953AC2655637D088F83ED]]></sign></xml>";
        }
    }

}
