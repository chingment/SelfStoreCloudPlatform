using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.Tenpay
{
    public class TenpayUnifiedOrderApi : TenpayBasePostApi, ITenpayPostApi
    {
        private string _postData;

        public string ApiName
        {
            get
            {
                return "pay/unifiedorder";
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

        public TenpayUnifiedOrderApi(IWxConfig config, UnifiedOrder order)
        {
            SortedDictionary<string, object> sParams = new SortedDictionary<string, object>();

            sParams.Add("appid", config.AppId);//公众账号ID
            sParams.Add("mch_id", config.MchId);//商户号
            sParams.Add("nonce_str", CommonUtil.GetNonceStr());//随机字符串
            sParams.Add("notify_url", config.Notify_Url);//通知地址
            sParams.Add("trade_type", order.trade_type);//交易类型
            sParams.Add("spbill_create_ip", order.spbill_create_ip);//终端IP
            sParams.Add("out_trade_no", order.out_trade_no);//商户订单号
            sParams.Add("total_fee", order.total_fee);//标价金额
            sParams.Add("body", order.body);//商品描述   
            sParams.Add("time_expire", order.time_expire);//订单过期时间  
            if (order.trade_type == "JSAPI")
            {
                sParams.Add("openid", order.openid);//用户标识   
            }

            if (!string.IsNullOrEmpty(order.goods_tag))
            {
                sParams.Add("goods_tag", order.goods_tag);//商品优惠标识  
            }
            if (order.trade_type == "MWEB")
            {
                sParams.Add("scene_info", "{\"h5_info\": {\"type\":\"Wap\",\"wap_url\": \"http://mobile.17fanju.com\",\"wap_name\": \"贩聚社团\"}}");//场景信息
            }
            //sParams.Add("device_info", "WEB");//设备号
            //sParams.Add("sign_type", "");//签名类型
            //sParams.Add("detail", "");商品详情
            //sParams.Add("attach", "");附加数据
            //sParams.Add("fee_type", "");//标价币种
            //sParams.Add("time_start", "");//交易起始时间
            //sParams.Add("time_expire", "");//交易结束时间
            //sParams.Add("goods_tag", "");//订单优惠标记
            //sParams.Add("product_id", "");//商品ID
            //sParams.Add("limit_pay", "");//指定支付方式
            //sParams.Add("openid", "openid");//用户标识
            //sParams.Add("scene_info", "");//场景信息
            string sign = MakeSign(sParams, config.Key);
            sParams.Add("sign", sign);//签名


            _postData = GetXml(sParams);

        }
    }
}
