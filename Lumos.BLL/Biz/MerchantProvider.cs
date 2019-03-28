using Lumos.WeiXinSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class MerchantProvider : BaseProvider
    {
        public WxAppInfoConfig GetWxPaAppInfoConfig(string id)
        {

            var config = new WxAppInfoConfig();

            var merchant = CurrentDb.Merchant.Where(m => m.Id == id).FirstOrDefault();
            if (merchant == null)
                return null;


            config.AppId = merchant.WechatPaAppId;
            config.AppSecret = merchant.WechatPaAppSecret;
            config.PayMchId = merchant.WechatPayMchId;
            config.PayKey = merchant.WechatPayKey;
            config.NotifyEventUrlToken = merchant.WechatPaNotifyEventUrlToken;
            config.Oauth2RedirectUrl = merchant.WechatPaOauth2RedirectUrl;
            config.PayResultNotifyUrl = merchant.WechatPayResultNotifyUrl;

            return config;
        }
    }
}
