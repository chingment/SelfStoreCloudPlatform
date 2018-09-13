using log4net;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lumos.WeiXinSdk;

namespace Lumos.BLL
{
    public sealed class WxUntil
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static WxUntil instance = null;
        private static readonly object padlock = new object();

        public static WxUntil GetInstance()
        {

            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new WxUntil();
                    }
                }
            }

            return instance;

        }



        public string GetAccessToken(string appId, string appSecret)
        {
            //用于测试配置
            string wxAccessToken = System.Configuration.ConfigurationManager.AppSettings["custom:WxTestAccessToken"];
            if (wxAccessToken != null)
            {
                return wxAccessToken;
            }

            string key = string.Format("Wx_AppId_{0}_AccessToken", appId);

            var redis = new RedisClient<string>();
            var accessToken = redis.KGetString(key);

            if (accessToken == null)
            {
                log.InfoFormat("获取微信AccessToken，key：{0}，已过期，重新获取", key);

                WxApi c = new WxApi();

                WxApiAccessToken apiAccessToken = new WxApiAccessToken("client_credential", appId, appSecret);

                var apiAccessTokenResult = c.DoGet(apiAccessToken);

                if (string.IsNullOrEmpty(apiAccessTokenResult.access_token))
                {
                    log.InfoFormat("获取微信AccessToken，key：{0}，已过期，Api重新获取失败", key);
                }
                else
                {
                    log.InfoFormat("获取微信AccessToken，key：{0}，value：{1}，已过期，重新获取成功", key, apiAccessTokenResult.access_token);

                    accessToken = apiAccessTokenResult.access_token;

                    redis.KSet(key, accessToken, new TimeSpan(0, 30, 0));
                }

            }
            else
            {
                log.InfoFormat("获取微信AccessToken，key：{0}，value：{1}", key, accessToken);
            }

            return accessToken;
        }


        public string GetJsApiTicket(string appId, string appSecret)
        {

            string key = string.Format("Wx_AppId_{0}_JsApiTicket", appId);

            var redis = new RedisClient<string>();
            var jsApiTicket = redis.KGetString(key);

            if (jsApiTicket == null)
            {
                WxApi c = new WxApi();

                string access_token = GetAccessToken(appId, appSecret);

                var wxApiJsApiTicket = new WxApiJsApiTicket(access_token);

                var wxApiJsApiTicketResult = c.DoGet(wxApiJsApiTicket);
                if (string.IsNullOrEmpty(wxApiJsApiTicketResult.ticket))
                {
                    log.InfoFormat("获取微信JsApiTicket，key：{0}，已过期，Api重新获取失败", key);
                }
                else
                {
                    log.InfoFormat("获取微信JsApiTicket，key：{0}，value：{1}，已过期，重新获取成功", key, wxApiJsApiTicketResult.ticket);

                    jsApiTicket = wxApiJsApiTicketResult.ticket;

                    redis.KSet(key, jsApiTicket, new TimeSpan(0, 30, 0));
                }
            }
            else
            {
                log.InfoFormat("获取微信JsApiTicket，key：{0}，value：{1}", key, jsApiTicket);
            }

            return jsApiTicket;

        }


        public string GetCardApiTicket(string appId, string appSecret)
        {

            string key = string.Format("Wx_AppId_{0}_CardApiTicket", appId);

            var redis = new RedisClient<string>();
            var jsApiTicket = redis.KGetString(key);

            if (jsApiTicket == null)
            {
                WxApi c = new WxApi();

                string access_token = GetAccessToken(appId, appSecret);

                var wxApiGetCardApiTicket = new WxApiGetCardApiTicket(access_token);

                var wxApiGetCardApiTicketResult = c.DoGet(wxApiGetCardApiTicket);
                if (string.IsNullOrEmpty(wxApiGetCardApiTicketResult.ticket))
                {
                    log.InfoFormat("获取微信JsApiTicket，key：{0}，已过期，Api重新获取失败", key);
                }
                else
                {
                    log.InfoFormat("获取微信JsApiTicket，key：{0}，value：{1}，已过期，重新获取成功", key, wxApiGetCardApiTicketResult.ticket);

                    jsApiTicket = wxApiGetCardApiTicketResult.ticket;

                    redis.KSet(key, jsApiTicket, new TimeSpan(0, 30, 0));
                }
            }
            else
            {
                log.InfoFormat("获取微信JsApiTicket，key：{0}，value：{1}", key, jsApiTicket);
            }

            return jsApiTicket;

        }

    }
}
