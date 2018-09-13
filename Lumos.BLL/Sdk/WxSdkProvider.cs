using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;
using Lumos.WeiXinSdk;
using Lumos.WeiXinSdk.Tenpay;
using System.Security.Cryptography;

namespace Lumos.BLL
{
    public class WxSdkProvider : BaseProvider
    {
        private IWxConfig wxConfig = new WxConfigByTest();
        public WxSdkProvider Instance()
        {
            WxSdkProvider p = new WxSdkProvider();
            p.Config = new WxConfigByQyj();
            //switch (merchantId)
            //{
            //    case 1:
            //        p.Config = new WxConfigByFanJu();
            //        break;
            //    case 2:
            //        p.Config = new WxConfigByFuLi();
            //        break;
            //}

            return p;
        }

        public IWxConfig Config
        {
            get
            {
                return wxConfig;
            }
            set
            {
                wxConfig = value;
            }
        }

        public string GetPrepayId(string operater, string trade_type, string openid, string orderSn, decimal orderAmount, string goods_tag, string ip, string body, DateTime? time_expire = null)
        {
            CustomJsonResult result = new CustomJsonResult();


            TenpayUtil tenpayUtil = new TenpayUtil(wxConfig);

            UnifiedOrder unifiedOrder = new UnifiedOrder();
            unifiedOrder.openid = openid;
            unifiedOrder.out_trade_no = orderSn;//商户订单号
            unifiedOrder.spbill_create_ip = ip;//终端IP
            unifiedOrder.total_fee = Convert.ToInt32(orderAmount * 100);//标价金额
            unifiedOrder.body = body;//商品描述  
            unifiedOrder.trade_type = trade_type;
            if (time_expire != null)
            {
                unifiedOrder.time_expire = time_expire.Value.ToString("yyyyMMddHHmmss");
            }

            if (!string.IsNullOrEmpty(goods_tag))
            {
                unifiedOrder.goods_tag = goods_tag;
            }

            string prepayId = tenpayUtil.GetPrepayId(unifiedOrder);

            //using (TransactionScope ts = new TransactionScope())
            //{

            //    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            //}

            return prepayId;
        }

        public string GetAuthorizeUrl(string returnUrl)
        {
            return OAuthApi.GetAuthorizeUrl(wxConfig.AppId, string.Format(wxConfig.Oauth2RedirectUrl, returnUrl));
        }

        public WxApiSnsOauth2AccessTokenResult GetWebOauth2AccessToken(string code)
        {
            return OAuthApi.GetWebOauth2AccessToken(wxConfig.AppId, wxConfig.AppSecret, code);
        }

        public string GetApiAccessToken()
        {
            return WxUntil.GetInstance().GetAccessToken(wxConfig.AppId, wxConfig.AppSecret);
        }

        public WxApiSnsUserInfoResult GetUserInfo(string accessToken, string openId)
        {
            return OAuthApi.GetUserInfo(accessToken, openId);
        }

        public string CardCodeDecrypt(string encrypt_code)
        {
            return OAuthApi.CardCodeDecrypt(this.GetApiAccessToken(), encrypt_code);
        }

        public WxApiUserInfoResult GetUserInfoByApiToken(string openId)
        {
            return OAuthApi.GetUserInfoByApiToken(this.GetApiAccessToken(), openId);
        }

        public CustomJsonResult<JsApiConfigParams> GetJsApiConfigParams(string url)
        {
            string jsApiTicket = WxUntil.GetInstance().GetJsApiTicket(wxConfig.AppId, wxConfig.AppSecret);

            JsApiConfigParams parms = new JsApiConfigParams(wxConfig.AppId, url, jsApiTicket);

            return new CustomJsonResult<JsApiConfigParams>(ResultType.Success, ResultCode.Success, "", parms);
        }

        public CustomJsonResult GetJsApiPayParams(string prepayId, string orderId, string orderSn)
        {
            CustomJsonResult result = new CustomJsonResult();
            JsApiPayParams parms = new JsApiPayParams(wxConfig.AppId, wxConfig.Key, prepayId, orderId, orderSn);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "", parms);
        }

        public string GetNotifyEventUrlToken()
        {
            return wxConfig.NotifyEventUrlToken;
        }

        public string GetCardApiTicket()
        {
            string cardApiTicket = WxUntil.GetInstance().GetCardApiTicket(wxConfig.AppId, wxConfig.AppSecret);
            return cardApiTicket;
        }


        public string UploadMultimediaImage(string imageUrl)
        {
            return OAuthApi.UploadMultimediaImage(this.GetApiAccessToken(), imageUrl);
        }



        public string OrderQuery(string orderSn)
        {
            CustomJsonResult result = new CustomJsonResult();
            TenpayUtil tenpayUtil = new TenpayUtil(wxConfig);
            string xml = tenpayUtil.OrderQuery(orderSn);

            return xml;
        }

        public bool CheckPayNotifySign(string xml)
        {

            var dic1 = WeiXinSdk.CommonUtil.ToDictionary(xml);

            if (dic1["sign"] == null)
            {
                return false;
            }

            string wxSign = dic1["sign"].ToString();


            bool isFlag = true;
            string buff = "";
            foreach (KeyValuePair<string, object> pair in dic1)
            {
                if (pair.Value == null)
                {
                    isFlag = false;
                    break;
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }

            if (!isFlag)
            {
                return false;
            }

            buff = buff.Trim('&');


            //在string后加入API KEY
            buff += "&key=" + wxConfig.Key;
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(buff));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            string mySign = sb.ToString().ToUpper();

            if (wxSign != mySign)
            {
                return false;
            }
            else
            {
                return true;
            }

        }




        //public string OrderPayReFund(string comCode, string orderSn, string orderReFundSn, decimal totalFee, decimal refundFee, string refundDesc)
        //{
        //    wxConfig = GetWxConfig(comCode);
        //    CustomJsonResult result = new CustomJsonResult();
        //    TenpayUtil tenpayUtil = new TenpayUtil(wxConfig);
        //    string out_trade_no = orderSn;
        //    string out_refund_no = orderReFundSn;
        //    string total_fee = Convert.ToInt32(totalFee * 100).ToString();
        //    string refund_fee = Convert.ToInt32(refundFee * 100).ToString();
        //    string xml = tenpayUtil.OrderPayReFund(out_trade_no, out_refund_no, total_fee, refund_fee, refundDesc);
        //    return xml;
        //}

        //public string OrderRefundQuery(string comCode, string out_refund_no)
        //{
        //    wxConfig = GetWxConfig(comCode);
        //    CustomJsonResult result = new CustomJsonResult();
        //    TenpayUtil tenpayUtil = new TenpayUtil(wxConfig);
        //    string xml = tenpayUtil.OrderRefundQuery(out_refund_no);
        //    return xml;
        //}
    }
}
