using Lumos.WeiXinSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WeiXinSdkWebTest.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public string Get()
        {

            string appid = "wx60443bbfd97f6aa5";
            string secret = "dfd062f723c38519092dfd7646af2f76";


            var request = HttpContext.Current.Request;
            var code = request.QueryString["code"];
            string str = "";
            if (string.IsNullOrEmpty(code))
            {
                string redirect_uri = HttpUtility.UrlEncode("http://www.feytil.com/api/Values");
                var url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", appid);
                HttpContext.Current.Response.Redirect(url);
            }
            else
            {
                WxApi api = new WxApi();
                WxApiSnsOauth2AccessToken oauth2AccessToken = new WxApiSnsOauth2AccessToken(appid, secret, code, "authorization_code");
                var result = api.DoGet(oauth2AccessToken);
                str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                if (result.errcode == null)
                {
                    WxApiSnsUserInfo snsUserInfo = new WxApiSnsUserInfo(result.access_token, result.openid, "zh_CN");
                    var result2 = api.DoGet(snsUserInfo);
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(result2);
                   
                }
            }



            return str;
        }

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
