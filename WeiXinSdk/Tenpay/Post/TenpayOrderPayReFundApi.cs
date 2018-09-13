using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.Tenpay
{
    public class TenpayOrderPayReFundApi : TenpayBasePostApi, ITenpayPostApi
    {
        private string _postData;

        public string ApiName
        {
            get
            {
                return "secapi/pay/refund";
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
        public TenpayOrderPayReFundApi(IWxConfig config, string out_trade_no, string out_refund_no, string total_fee, string refund_fee,string refund_desc)
        {
            SortedDictionary<string, object> sParams = new SortedDictionary<string, object>();

            sParams.Add("appid", config.AppId);//公众账号ID
            sParams.Add("mch_id", config.MchId);//商户号
            sParams.Add("nonce_str", CommonUtil.GetNonceStr());//随机字符串
            sParams.Add("out_trade_no", out_trade_no);//微信订单号



            sParams.Add("total_fee", int.Parse(total_fee));//订单总金额
            sParams.Add("refund_fee", int.Parse(refund_fee));//退款金额
            sParams.Add("out_refund_no", out_refund_no);//随机生成商户退款单号
            sParams.Add("refund_desc", refund_desc);//退款原因

            string sign = MakeSign(sParams, config.Key);
            sParams.Add("sign", sign);//签名

            _postData = GetXml(sParams);

        }
    }
}
