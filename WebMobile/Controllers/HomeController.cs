﻿using System;
using System.Web;
using System.Web.Mvc;
using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.WeiXinSdk;
using System.Text;
using System.Web.Security;
using WebMobile.Models.Home;
using Lumos.WeiXinSdk.MsgPush;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Lumos;
using Lumos.Session;
using ZXing;
using ZXing.QrCode;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WebMobile.Controllers
{
    public class HomeController : OwnBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        //获取JsApiConfig配置参数
        public CustomJsonResult<JsApiConfigParams> GetJsApiConfigParams(string url)
        {
            return SdkFactory.Wx.Instance().GetJsApiConfigParams(url);
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

                if (string.IsNullOrEmpty(code))
                {
                    var url = SdkFactory.Wx.Instance().GetAuthorizeUrl(returnUrl);

                    LogUtil.Info("待跳转路径2：" + url);

                    return Redirect(url);
                }
                else
                {
                    var oauth2_Result = SdkFactory.Wx.Instance().GetWebOauth2AccessToken(code);
                    if (oauth2_Result.errcode == null)
                    {
                        LogUtil.Info("用户OpenId:" + oauth2_Result.openid);
                        LogUtil.Info("用户AccessToken:" + oauth2_Result.access_token);

                        var snsUserInfo_Result = SdkFactory.Wx.Instance().GetUserInfo(oauth2_Result.access_token, oauth2_Result.openid);
                        WxUserInfo wxUserInfo = new WxUserInfo();
                        wxUserInfo.AccessToken = oauth2_Result.access_token;
                        wxUserInfo.OpenId = oauth2_Result.openid;
                        wxUserInfo.ExpiresIn = DateTime.Now.AddSeconds(oauth2_Result.expires_in);
                        wxUserInfo.Nickname = snsUserInfo_Result.nickname;
                        wxUserInfo.Sex = snsUserInfo_Result.sex;
                        wxUserInfo.Province = snsUserInfo_Result.province;
                        wxUserInfo.City = snsUserInfo_Result.city;
                        wxUserInfo.Country = snsUserInfo_Result.country;
                        wxUserInfo.HeadImgUrl = snsUserInfo_Result.headimgurl;
                        wxUserInfo.UnionId = snsUserInfo_Result.unionid;


                        wxUserInfo = BizFactory.WxUser.CheckedUser(GuidUtil.New(), wxUserInfo);
                        if (wxUserInfo != null)
                        {
                            LogUtil.Info("用户Id：" + wxUserInfo.UserId);

                            UserInfo userInfo = new UserInfo();
                            userInfo.Token = GuidUtil.New();
                            userInfo.UserId = wxUserInfo.UserId;
                            userInfo.WxOpenId = oauth2_Result.openid;
                            userInfo.WxAccessToken = oauth2_Result.access_token;
                            SSOUtil.SetUserInfo(userInfo);
                            Response.Cookies.Add(new HttpCookie(OwnRequest.SESSION_NAME, userInfo.Token));

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
            Int32 intLen = Convert.ToInt32(Request.InputStream.Length);
            byte[] b = new byte[intLen];
            Request.InputStream.Read(b, 0, intLen);

            string xml = System.Text.Encoding.UTF8.GetString(b);

            if (string.IsNullOrEmpty(xml))
            {
                return Content("");
            }

            LogUtil.Info("接收支付结果:" + xml);

            if (!SdkFactory.Wx.Instance().CheckPayNotifySign(xml))
            {
                LogUtil.Warn("支付通知结果签名验证失败");
                return Content("");
            }

            string orderSn = "";
            var dic2 = Lumos.WeiXinSdk.CommonUtil.ToDictionary(xml);
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
            sb.Append("浏览器agent：" + Lumos.Common.CommonUtils.GetBrowserInfo() + "\t\n");
            LogUtil.Error(sb.ToString());
            return res;

        }

        [AllowAnonymous]
        public ActionResult NotifyEvent()
        {
            LogUtil.Info("开始接收事件推送通知");

            if (Request.HttpMethod == "POST")
            {
                Int32 intLen = Convert.ToInt32(Request.InputStream.Length);
                byte[] b = new byte[intLen];
                Request.InputStream.Read(b, 0, intLen);
                string xml = System.Text.Encoding.UTF8.GetString(b);

                LogUtil.Info("接收事件推送内容:" + xml);

                var baseEventMsg = WxMsgFactory.CreateMessage(xml);
                string echoStr = "";
                string eventKey = null;
                LogUtil.Info("baseEventMsg内容:" + baseEventMsg);
                if (baseEventMsg != null)
                {
                    var userInfo_Result = SdkFactory.Wx.Instance().GetUserInfoByApiToken(baseEventMsg.FromUserName);

                    if (userInfo_Result.openid != null)
                    {
                        LogUtil.Info("userInfo_Result:" + JsonConvert.SerializeObject(userInfo_Result));

                        var wxUserInfo = new WxUserInfo();

                        wxUserInfo.OpenId = userInfo_Result.openid;
                        wxUserInfo.Nickname = userInfo_Result.nickname;
                        wxUserInfo.Sex = userInfo_Result.sex.ToString();
                        wxUserInfo.Province = userInfo_Result.province;
                        wxUserInfo.City = userInfo_Result.city;
                        wxUserInfo.Country = userInfo_Result.country;
                        wxUserInfo.HeadImgUrl = userInfo_Result.headimgurl;
                        wxUserInfo.UnionId = userInfo_Result.unionid;

                        wxUserInfo = BizFactory.WxUser.CheckedUser(GuidUtil.New(), wxUserInfo);

                        if (wxUserInfo != null)
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
                            wxMsgPushLog.UserId = wxUserInfo.UserId;
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
            string[] ArrTmp = { SdkFactory.Wx.Instance().GetNotifyEventUrlToken(), timestamp, nonce };
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