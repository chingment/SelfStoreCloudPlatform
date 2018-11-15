using System;
using System.Web;
using System.Web.Mvc;
using Lumos.BLL;
using Lumos.Entity;
using Lumos.Web.Mvc;
using Lumos.WeiXinSdk;
using System.Text;
using System.Web.Security;
using Lumos.WeiXinSdk.MsgPush;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Lumos;
using Lumos.Session;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Lumos.BLL.Biz.RModels;
using Lumos.BLL.Biz;
using Lumos.BLL.Service.Admin.Sys;
using Lumos.BLL.Service.Admin;

namespace WebMobile.Controllers
{
    public class HomeController : OwnBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        //获取JsApiConfig配置参数
        public CustomJsonResult<JsApiConfigParams> GetJsApiConfigParams(string url)
        {
            return SdkFactory.Wx.GetJsApiConfigParams(this.CurrentAppInfo, url);
        }

        [AllowAnonymous]
        public RedirectResult Oauth2()
        {
            try
            {
                var request = Request;
                var code = request.QueryString["code"];
                var returnUrl = request.QueryString["returnUrl"];

                LogUtil.Info("returnUrl=>" + (returnUrl == null ? "" : returnUrl.ToString()));


                var appInfo = this.CurrentAppInfo;
                if (string.IsNullOrEmpty(code))
                {
                    var url = SdkFactory.Wx.GetWebAuthorizeUrl(appInfo, returnUrl);
                    LogUtil.Info("待跳转路径2：" + url);
                    return Redirect(url);
                }
                else
                {
                    var oauth2_Result = SdkFactory.Wx.GetWebOauth2AccessToken(appInfo, code);
                    if (oauth2_Result.errcode == null)
                    {
                        LogUtil.Info("用户OpenId:" + oauth2_Result.openid);
                        LogUtil.Info("用户AccessToken:" + oauth2_Result.access_token);

                        var snsUserInfo_Result = SdkFactory.Wx.GetUserInfo(oauth2_Result.access_token, oauth2_Result.openid);


                        RopWxUserCheckedUser ropWxUserCheckedUser = new RopWxUserCheckedUser();
                        ropWxUserCheckedUser.AccessToken = oauth2_Result.access_token;
                        ropWxUserCheckedUser.OpenId = oauth2_Result.openid;
                        ropWxUserCheckedUser.ExpiresIn = DateTime.Now.AddSeconds(oauth2_Result.expires_in);
                        ropWxUserCheckedUser.Nickname = snsUserInfo_Result.nickname;
                        ropWxUserCheckedUser.Sex = snsUserInfo_Result.sex;
                        ropWxUserCheckedUser.Province = snsUserInfo_Result.province;
                        ropWxUserCheckedUser.City = snsUserInfo_Result.city;
                        ropWxUserCheckedUser.Country = snsUserInfo_Result.country;
                        ropWxUserCheckedUser.HeadImgUrl = snsUserInfo_Result.headimgurl;
                        ropWxUserCheckedUser.UnionId = snsUserInfo_Result.unionid;


                        var retWxUserCheckedUser = BizFactory.WxUser.CheckedUser(GuidUtil.New(), ropWxUserCheckedUser);
                        if (retWxUserCheckedUser != null)
                        {
                            LogUtil.Info("用户Id：" + retWxUserCheckedUser.ClientId);

                            UserInfo userInfo = new UserInfo();
                            userInfo.UserId = retWxUserCheckedUser.ClientId;
                            userInfo.WxOpenId = oauth2_Result.openid;
                            userInfo.WxAccessToken = oauth2_Result.access_token;

                            string accessToken = GuidUtil.New();

                            SSOUtil.SetUserInfo(accessToken, userInfo);
                            Response.Cookies.Add(new HttpCookie(OwnRequest.SESSION_NAME, accessToken));

                            LogUtil.Info("returnUrl.UrlDecode 前：" + returnUrl);
                            string s_returnUrl = HttpUtility.UrlDecode(returnUrl);
                            LogUtil.Info("returnUrl.UrlDecode 后：" + s_returnUrl);
                            s_returnUrl = s_returnUrl.Replace("|", "&");
                            LogUtil.Info("returnUrl.UrlDecode 替换|，&：" + s_returnUrl);

                            LogUtil.Info("returnUrl 最后返回：" + s_returnUrl);

                            if (!string.IsNullOrEmpty(s_returnUrl))
                            {
                                return Redirect(s_returnUrl);
                            }
                        }

                        LogUtil.Info("用户跳进主页");

                        return Redirect("/Home/Index");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error("微信Oauth2授权验证发生异常", ex);
            }

            return Redirect("/Home/Oauth2");
        }

        [AllowAnonymous]
        public ContentResult PayResult()
        {
            Stream stream = Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string xml = new StreamReader(stream).ReadToEnd();

            if (string.IsNullOrEmpty(xml))
            {
                return Content("");
            }

            LogUtil.Info("接收支付结果:" + xml);

            var dic2 = Lumos.WeiXinSdk.CommonUtil.ToDictionary(xml);
            if (!dic2.ContainsKey("appid"))
            {
                LogUtil.Warn("查找不到appid");
                return Content("");
            }

            string appId = dic2["appid"].ToString();

            var appInfo = AdminServiceFactory.AppInfo.Get(appId);

            if (!SdkFactory.Wx.CheckPayNotifySign(appInfo, xml))
            {
                LogUtil.Warn("支付通知结果签名验证失败");
                return Content("");
            }

            string orderSn = "";

            if (dic2.ContainsKey("out_trade_no") && dic2.ContainsKey("result_code"))
            {
                orderSn = dic2["out_trade_no"].ToString();
            }

            bool isPaySuccessed = false;
            var result = BizFactory.Order.PayResultNotify(GuidUtil.Empty(), Enumeration.OrderNotifyLogNotifyFrom.NotifyUrl, xml, orderSn, out isPaySuccessed);

            if (result.Result == ResultType.Success)
            {
                Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
                Response.End();
            }

            return Content("");
        }

        [AllowAnonymous]
        public ActionResult LogJsError(string errorMessage, string scriptURI, string columnNumber, string errorObj)
        {
            CustomJsonResult res = new CustomJsonResult();
            StringBuilder sb = new StringBuilder();
            sb.Append("前端JS脚本错误：" + errorMessage + "\t\n");
            sb.Append("错误信息：" + errorMessage + "\t\n");
            sb.Append("出错文件：" + scriptURI + "\t\n");
            sb.Append("出错列号：" + columnNumber + "\t\n");
            sb.Append("错误详情：" + errorObj + "\t\n");
            sb.Append("浏览器agent：" + Lumos.Common.CommonUtil.GetBrowserInfo() + "\t\n");
            LogUtil.Error(sb.ToString());
            return res;

        }

        [AllowAnonymous]
        public ActionResult NotifyEvent()
        {
            LogUtil.Info("开始接收事件推送通知");

            if (Request.HttpMethod == "POST")
            {

                Stream stream = Request.InputStream;
                stream.Seek(0, SeekOrigin.Begin);
                string xml = new StreamReader(stream).ReadToEnd();

                LogUtil.Info("接收事件推送内容:" + xml);

                var baseEventMsg = WxMsgFactory.CreateMessage(xml);
                string echoStr = "";
                string eventKey = null;
                LogUtil.Info("baseEventMsg内容:" + baseEventMsg);
                if (baseEventMsg != null)
                {
                    var appId = Request.QueryString["appId"];
                    var appInfo = AdminServiceFactory.AppInfo.Get(appId);
                    var userInfo_Result = SdkFactory.Wx.GetUserInfoByApiToken(appInfo, baseEventMsg.FromUserName);

                    if (userInfo_Result.openid != null)
                    {
                        LogUtil.Info("userInfo_Result:" + JsonConvert.SerializeObject(userInfo_Result));

                        var ropWxUserCheckedUser = new RopWxUserCheckedUser();

                        ropWxUserCheckedUser.OpenId = userInfo_Result.openid;
                        ropWxUserCheckedUser.Nickname = userInfo_Result.nickname;
                        ropWxUserCheckedUser.Sex = userInfo_Result.sex.ToString();
                        ropWxUserCheckedUser.Province = userInfo_Result.province;
                        ropWxUserCheckedUser.City = userInfo_Result.city;
                        ropWxUserCheckedUser.Country = userInfo_Result.country;
                        ropWxUserCheckedUser.HeadImgUrl = userInfo_Result.headimgurl;
                        ropWxUserCheckedUser.UnionId = userInfo_Result.unionid;

                        var retWxUserCheckedUser = BizFactory.WxUser.CheckedUser(GuidUtil.New(), ropWxUserCheckedUser);

                        if (retWxUserCheckedUser != null)
                        {
                            var wxAutoReply = new WxAutoReply();
                            switch (baseEventMsg.MsgType)
                            {
                                case MsgType.TEXT:
                                    #region TEXT

                                    LogUtil.Info("文本消息");

                                    var textMsg = (TextMsg)baseEventMsg;

                                    if (textMsg != null)
                                    {
                                        LogUtil.Info("文本消息:" + textMsg.Content);
                                    }


                                    #endregion
                                    break;
                                case MsgType.EVENT:
                                    #region EVENT
                                    switch (baseEventMsg.Event)
                                    {
                                        case EventType.SUBSCRIBE://订阅
                                            break;
                                        case EventType.UNSUBSCRIBE://取消订阅
                                            break;
                                        case EventType.SCAN://扫描二维码
                                        case EventType.CLICK://单击按钮
                                        case EventType.VIEW://链接按钮
                                            break;
                                        case EventType.USER_GET_CARD://领取卡卷
                                            #region  USER_GET_CARD


                                            #endregion
                                            break;
                                        case EventType.USER_CONSUME_CARD://核销卡卷
                                            #region USER_CONSUME_CARD


                                            #endregion
                                            break;
                                    }
                                    #endregion
                                    break;
                            }

                            var wxMsgPushLog = new WxMsgPushLog();
                            wxMsgPushLog.Id = GuidUtil.New();
                            wxMsgPushLog.UserId = retWxUserCheckedUser.ClientId;
                            wxMsgPushLog.ToUserName = baseEventMsg.ToUserName;
                            wxMsgPushLog.FromUserName = baseEventMsg.FromUserName;
                            wxMsgPushLog.CreateTime = DateTime.Now;
                            wxMsgPushLog.ContentXml = xml;
                            wxMsgPushLog.MsgId = baseEventMsg.MsgId;
                            wxMsgPushLog.MsgType = baseEventMsg.MsgType.ToString();
                            wxMsgPushLog.Event = baseEventMsg.Event.ToString();
                            wxMsgPushLog.EventKey = eventKey;

                            WxMsgPushLog(wxMsgPushLog);
                        }
                    }
                }

                LogUtil.Info(string.Format("接收事件推送之后回复内容:{0}", echoStr));

                Response.Write(echoStr);
            }
            else if (Request.HttpMethod == "GET") //微信服务器在首次验证时，需要进行一些验证，但。。。。  
            {
                if (string.IsNullOrEmpty(Request["echostr"]))
                {
                    Response.Write("无法获取微信接入信息，仅供测试！");

                }

                Response.Write(Request["echostr"].ToString());
            }
            else
            {
                Response.Write("wrong");
            }

            Response.End();

            return View();
        }

        public Task<bool> WxMsgPushLog(WxMsgPushLog wxMsgPushLog)
        {
            return Task.Run(() =>
            {

                CurrentDb.WxMsgPushLog.Add(wxMsgPushLog);
                CurrentDb.SaveChanges();


                return true;

            });
        }

        private bool CheckSignature()
        {
            string signature = Request.QueryString["signature"].ToString();
            string timestamp = Request.QueryString["timestamp"].ToString();
            string nonce = Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { SdkFactory.Wx.GetNotifyEventUrlToken(this.CurrentAppInfo), timestamp, nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Bitmap CirclePhoto(string urlPath, int size)
        {

            try
            {
                System.Net.WebRequest webreq = System.Net.WebRequest.Create(urlPath);
                System.Net.WebResponse webres = webreq.GetResponse();
                System.IO.Stream stream = webres.GetResponseStream();
                Image img1 = System.Drawing.Image.FromStream(stream);
                stream.Dispose();

                Bitmap b = new Bitmap(size, size);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.DrawImage(img1, 0, 0, b.Width, b.Height);
                    int r = Math.Min(b.Width, b.Height) / 2;
                    PointF c = new PointF(b.Width / 2.0F, b.Height / 2.0F);
                    for (int h = 0; h < b.Height; h++)
                        for (int w = 0; w < b.Width; w++)
                            if ((int)Math.Pow(r, 2) < ((int)Math.Pow(w * 1.0 - c.X, 2) + (int)Math.Pow(h * 1.0 - c.Y, 2)))
                            {
                                b.SetPixel(w, h, Color.Transparent);
                            }
                    //画背景色圆
                    using (Pen p = new Pen(System.Drawing.SystemColors.Control))
                        g.DrawEllipse(p, 0, 0, b.Width, b.Height);
                }
                return b;
            }
            catch (Exception ex)
            {
                LogUtil.Error("CirclePhoto生成发生异常", ex);

                return null;
            }

        }

    }
}