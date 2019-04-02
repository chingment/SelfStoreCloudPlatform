using Lumos.Entity;
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
        private string AES_decrypt(string encryptedData, string iv, string sessionKey)
        {
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                //设置解密器参数            
                aes.Mode = CipherMode.CBC;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                //格式化待处理字符串          
                byte[] byte_encryptedData = Convert.FromBase64String(encryptedData);
                byte[] byte_iv = Convert.FromBase64String(iv);
                byte[] byte_sessionKey = Convert.FromBase64String(sessionKey);
                aes.IV = byte_iv;
                aes.Key = byte_sessionKey;
                //根据设置好的数据生成解密器实例            
                ICryptoTransform transform = aes.CreateDecryptor();
                //解密          
                byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);
                //生成结果         
                string result = Encoding.UTF8.GetString(final);


                return result;
            }
            catch
            {
                return null;
            }

        }

        public string GetJsApiTicket(WxAppInfoConfig config)
        {

            string key = string.Format("Wx_AppId_{0}_JsApiTicket", config.AppId);

            var redis = new RedisClient<string>();
            var jsApiTicket = redis.KGetString(key);

            if (jsApiTicket == null)
            {
                WxApi c = new WxApi();

                string access_token = GetApiAccessToken(config);

                var wxApiJsApiTicket = new WxApiJsApiTicket(access_token);

                var wxApiJsApiTicketResult = c.DoGet(wxApiJsApiTicket);
                if (string.IsNullOrEmpty(wxApiJsApiTicketResult.ticket))
                {
                    LogUtil.Info(string.Format("获取微信JsApiTicket，key：{0}，已过期，Api重新获取失败", key));
                }
                else
                {
                    LogUtil.Info(string.Format("获取微信JsApiTicket，key：{0}，value：{1}，已过期，重新获取成功", key, wxApiJsApiTicketResult.ticket));

                    jsApiTicket = wxApiJsApiTicketResult.ticket;

                    redis.KSet(key, jsApiTicket, new TimeSpan(0, 30, 0));
                }
            }
            else
            {
                LogUtil.Info(string.Format("获取微信JsApiTicket，key：{0}，value：{1}", key, jsApiTicket));
            }

            return jsApiTicket;

        }

        public UnifiedOrderResult UnifiedOrderByJsApi(WxAppInfoConfig config, string openId, string orderSn, decimal orderAmount, string goods_tag, string ip, string body, DateTime? time_expire = null)
        {

            var ret = new UnifiedOrderResult();

            TenpayUtil tenpayUtil = new TenpayUtil(config);

            UnifiedOrder unifiedOrder = new UnifiedOrder();
            unifiedOrder.openid = openId;
            unifiedOrder.out_trade_no = orderSn;//商户订单号
            unifiedOrder.spbill_create_ip = "192.168.1.1";//终端IP
            unifiedOrder.total_fee = Convert.ToInt32(orderAmount * 100);//标价金额
            unifiedOrder.body = body;//商品描述  
            unifiedOrder.trade_type = "JSAPI";
            if (time_expire != null)
            {
                unifiedOrder.time_expire = time_expire.Value.ToString("yyyyMMddHHmmss");
            }

            if (!string.IsNullOrEmpty(goods_tag))
            {
                unifiedOrder.goods_tag = goods_tag;
            }

            ret = tenpayUtil.UnifiedOrder(unifiedOrder);

            return ret;

        }

        public UnifiedOrderResult UnifiedOrderByNative(WxAppInfoConfig config, string merchantId, string orderSn, decimal orderAmount, string goods_tag, string ip, string body, DateTime time_expire)
        {

            var ret = new UnifiedOrderResult();

            TenpayUtil tenpayUtil = new TenpayUtil(config);

            UnifiedOrder unifiedOrder = new UnifiedOrder();
            unifiedOrder.openid = "";
            unifiedOrder.out_trade_no = orderSn;//商户订单号
            unifiedOrder.spbill_create_ip = ip;//终端IP
            unifiedOrder.total_fee = Convert.ToInt32(orderAmount * 100);//标价金额
            unifiedOrder.body = body;//商品描述  
            unifiedOrder.trade_type = "NATIVE";
            unifiedOrder.time_expire = time_expire.ToString("yyyyMMddHHmmss");
            unifiedOrder.goods_tag = goods_tag;
            unifiedOrder.attach = "{\"merchantId\":\"" + merchantId + "\"}";
            ret = tenpayUtil.UnifiedOrder(unifiedOrder);

            return ret;

        }

        public string GetWebAuthorizeUrl(WxAppInfoConfig config, string returnUrl)
        {
            return OAuthApi.GetAuthorizeUrl(config.AppId, config.Oauth2RedirectUrl + "?returnUrl=" + returnUrl);
        }

        public WxApiSnsOauth2AccessTokenResult GetWebOauth2AccessToken(WxAppInfoConfig config, string code)
        {
            return OAuthApi.GetWebOauth2AccessToken(config.AppId, config.AppSecret, code);
        }

        public string GetApiAccessToken(WxAppInfoConfig config)
        {
            string wxAccessToken = System.Configuration.ConfigurationManager.AppSettings["custom:WxTestAccessToken"];
            if (wxAccessToken != null)
            {
                return wxAccessToken;
            }

            string key = string.Format("Wx_AppId_{0}_AccessToken", config.AppId);

            var redis = new RedisClient<string>();
            var accessToken = redis.KGetString(key);

            if (accessToken == null)
            {
                LogUtil.Info(string.Format("获取微信AccessToken，key：{0}，已过期，重新获取", key));

                WxApi c = new WxApi();

                WxApiAccessToken apiAccessToken = new WxApiAccessToken("client_credential", config.AppId, config.AppSecret);

                var apiAccessTokenResult = c.DoGet(apiAccessToken);

                if (string.IsNullOrEmpty(apiAccessTokenResult.access_token))
                {
                    LogUtil.Info(string.Format("获取微信AccessToken，key：{0}，已过期，Api重新获取失败", key));
                }
                else
                {
                    LogUtil.Info(string.Format("获取微信AccessToken，key：{0}，value：{1}，已过期，重新获取成功", key, apiAccessTokenResult.access_token));

                    accessToken = apiAccessTokenResult.access_token;

                    redis.KSet(key, accessToken, new TimeSpan(0, 30, 0));
                }

            }
            else
            {
                LogUtil.Info(string.Format("获取微信AccessToken，key：{0}，value：{1}", key, accessToken));
            }

            return accessToken;
        }

        public WxApiSnsUserInfoResult GetUserInfo(string accessToken, string openId)
        {
            return OAuthApi.GetUserInfo(accessToken, openId);
        }

        public UserInfoModelByMinProramJsCode GetUserInfoByMinProramJsCode(WxAppInfoConfig config, string encryptedData, string iv, string code)
        {
            try
            {
                var jsCode2Session = OAuthApi.GetWxApiJsCode2Session(config.AppId, config.AppSecret, code);
                string strData = AES_decrypt(encryptedData, iv, jsCode2Session.session_key);
                LogUtil.Info("UserInfo:" + strData);
                var obj = JsonConvert.DeserializeObject<UserInfoModelByMinProramJsCode>(strData);

                return obj;
            }
            catch
            {
                return null;
            }
        }

        public string CardCodeDecrypt(WxAppInfoConfig config, string encrypt_code)
        {
            return OAuthApi.CardCodeDecrypt(this.GetApiAccessToken(config), encrypt_code);
        }

        public WxApiUserInfoResult GetUserInfoByApiToken(WxAppInfoConfig config, string openId)
        {
            return OAuthApi.GetUserInfoByApiToken(this.GetApiAccessToken(config), openId);
        }

        public CustomJsonResult<JsApiConfigParams> GetJsApiConfigParams(WxAppInfoConfig config, string url)
        {
            string jsApiTicket = GetJsApiTicket(config);

            JsApiConfigParams parms = new JsApiConfigParams(config.AppId, url, jsApiTicket);

            return new CustomJsonResult<JsApiConfigParams>(ResultType.Success, ResultCode.Success, "", parms);
        }

        public JsApiPayParams GetJsApiPayParams(WxAppInfoConfig config, string orderId, string orderSn, string prepayId)
        {
            JsApiPayParams parms = new JsApiPayParams(config.AppId, config.PayKey, prepayId, orderId, orderSn);

            return parms;
        }

        public string GetNotifyEventUrlToken(WxAppInfoConfig config)
        {
            return config.NotifyEventUrlToken;
        }

        public string GetCardApiTicket(WxAppInfoConfig config)
        {

            string key = string.Format("Wx_AppId_{0}_CardApiTicket", config.AppId);

            var redis = new RedisClient<string>();
            var jsApiTicket = redis.KGetString(key);

            if (jsApiTicket == null)
            {
                WxApi c = new WxApi();

                string access_token = GetApiAccessToken(config);

                var wxApiGetCardApiTicket = new WxApiGetCardApiTicket(access_token);

                var wxApiGetCardApiTicketResult = c.DoGet(wxApiGetCardApiTicket);
                if (string.IsNullOrEmpty(wxApiGetCardApiTicketResult.ticket))
                {
                    LogUtil.Info(string.Format("获取微信JsApiTicket，key：{0}，已过期，Api重新获取失败", key));
                }
                else
                {
                    LogUtil.Info(string.Format("获取微信JsApiTicket，key：{0}，value：{1}，已过期，重新获取成功", key, wxApiGetCardApiTicketResult.ticket));

                    jsApiTicket = wxApiGetCardApiTicketResult.ticket;

                    redis.KSet(key, jsApiTicket, new TimeSpan(0, 30, 0));
                }
            }
            else
            {
                LogUtil.Info(string.Format("获取微信JsApiTicket，key：{0}，value：{1}", key, jsApiTicket));
            }

            return jsApiTicket;

        }

        public string UploadMultimediaImage(WxAppInfoConfig config, string imageUrl)
        {
            return OAuthApi.UploadMultimediaImage(this.GetApiAccessToken(config), imageUrl);
        }

        public string OrderQuery(WxAppInfoConfig config, string orderSn)
        {
            CustomJsonResult result = new CustomJsonResult();
            TenpayUtil tenpayUtil = new TenpayUtil(config);
            string xml = tenpayUtil.OrderQuery(orderSn);

            return xml;
        }

        public bool CheckPayNotifySign(WxAppInfoConfig config, string xml)
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
            buff += "&key=" + config.PayKey;
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
