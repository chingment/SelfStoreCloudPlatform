using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lumos.WeiXinSdk
{
    public static class OAuthApi
    {
        public static string GetAuthorizeUrl(string appId, string redirectUrl)
        {
            redirectUrl = redirectUrl.Replace("&", "|");
            redirectUrl = HttpUtility.UrlEncode(redirectUrl);
            LogUtil.Info("redirectUrl:" + redirectUrl);
            var url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri=" + redirectUrl + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", appId);

            return url;
        }

        public static WxApiSnsOauth2AccessTokenResult GetWebOauth2AccessToken(string appId, string secret, string code)
        {
            WxApi api = new WxApi();
            WxApiSnsOauth2AccessToken oauth2AccessToken = new WxApiSnsOauth2AccessToken(appId, secret, code, "authorization_code");
            var oauth2_Result = api.DoGet(oauth2AccessToken);
            return oauth2_Result;
        }

        public static WxApiSnsUserInfoResult GetUserInfo(string accessToken, string openId)
        {
            WxApi api = new WxApi();
            WxApiSnsUserInfo snsUserInfo = new WxApiSnsUserInfo(accessToken, openId, "zh_CN");
            var snsUserInfo_Result = api.DoGet(snsUserInfo);
            return snsUserInfo_Result;
        }

        public static WxApiUserInfoResult GetUserInfoByApiToken(string accessToken, string openId)
        {
            WxApi api = new WxApi();
            WxApiUserInfo snsUserInfo = new WxApiUserInfo(accessToken, openId);
            var userInfo_Result = api.DoGet(snsUserInfo);
            return userInfo_Result;
        }

        public static WxApiJsApiTicketResult GetJsApiTicket(string accessToken)
        {
            var api = new WxApi();
            var jsApiTicket = new WxApiJsApiTicket(accessToken);
            var snsUserInfo_Result = api.DoGet(jsApiTicket);
            return snsUserInfo_Result;
        }

        public static WxApiGetCardApiTicketResult GetCardApiTicket(string accessToken)
        {
            var api = new WxApi();
            var getCardApiTicket = new WxApiGetCardApiTicket(accessToken);
            var getCardApiTicket_Result = api.DoGet(getCardApiTicket);
            return getCardApiTicket_Result;
        }


        public static List<string> GetUserOpenIds(string accessToken)
        {
            var opendIds = new List<string>();
            GetUserOpenIds(ref opendIds, accessToken);
            return opendIds;
        }


        public static string UploadMultimediaImage(string accessToken, string imageUrl)
        {

            string mediaId = "";
            string wxurl = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + accessToken + "&type=image";
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                byte[] responseArray = myWebClient.UploadFile(wxurl, "POST", imageUrl);
                string str_result = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UploadMultimediaResult>(str_result);

                if (result != null)
                {
                    mediaId = result.media_id;
                }
            }
            catch (Exception ex)
            {
                mediaId = "Error:" + ex.Message;
            }
            return mediaId;

        }


        public static string CardCodeDecrypt(string accessToken, string encrypt_code)
        {
            var api = new WxApi();
            var wxApiCardCodeDecrpt = new WxApiCardCodeDecrpt(accessToken, WxPostDataType.Text, "{\"encrypt_code\":\"" + encrypt_code + "\"}");
            var wxApiCardCodeDecrpt_Result = api.DoPost(wxApiCardCodeDecrpt);
            return wxApiCardCodeDecrpt_Result.code;
        }

        private static void GetUserOpenIds(ref List<string> opendIds, string accessToken, string next_openid = "")
        {
            WxApi api = new WxApi();
            var userGet = new WxApiUserGet(accessToken, next_openid);
            var userGet_Result = api.DoGet(userGet);

            if (string.IsNullOrEmpty(userGet_Result.errcode))
            {
                if (userGet_Result.data != null)
                {
                    if (userGet_Result.data.openid != null)
                    {
                        opendIds.AddRange(userGet_Result.data.openid);
                    }

                    if (!string.IsNullOrEmpty(userGet_Result.next_openid))
                    {

                        GetUserOpenIds(ref opendIds, accessToken, userGet_Result.next_openid);
                    }
                }
            }

        }
    }
}
