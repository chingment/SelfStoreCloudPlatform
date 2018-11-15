
using System.Web.Http;
using Lumos;
using Lumos.WeiXinSdk;
using System.IO;
using System.Net.Http;
using Lumos.BLL;
using Lumos.Entity;
using System.Net;
using System.Text;
using System.Web;
using Lumos.BLL.Service.AppMobile;
using Lumos.BLL.Service.Admin;
using Lumos.BLL.Service.Merch;
using Lumos.BLL.Biz;

namespace WebAppApi.Controllers
{
    public class OrderController : OwnApiBaseController
    {
        [HttpPost]
        public OwnApiHttpResponse Confirm([FromBody]RopOrderConfirm rop)
        {
            IResult result = AppServiceFactory.Order.Confrim(this.CurrentUserId, this.CurrentUserId, rop);

            return new OwnApiHttpResponse(result);
        }


        [HttpPost]
        public OwnApiHttpResponse Reserve([FromBody]Lumos.BLL.Service.AppMobile.RopOrderReserve rop)
        {
            IResult result = AppServiceFactory.Order.Reserve(this.CurrentUserId, this.CurrentUserId, rop);
            return new OwnApiHttpResponse(result);
        }


        [HttpGet]
        public OwnApiHttpResponse GetJsApiPaymentPms([FromUri]RupOrderGetJsApiPaymentPms rop)
        {
            IResult result = AppServiceFactory.Order.GetJsApiPaymentPms(this.CurrentUserId, this.CurrentUserId, this.CurrentAppInfo, rop);
            return new OwnApiHttpResponse(result);
        }


        [AllowAnonymous]
        public HttpResponseMessage PayResult()
        {
            var myRequest = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request;
            Stream stream = myRequest.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string xml = new StreamReader(stream).ReadToEnd();

            if (string.IsNullOrEmpty(xml))
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("", Encoding.UTF8, "text/plain") };
            }

            LogUtil.Info("接收支付结果:" + xml);

            var dic2 = Lumos.WeiXinSdk.CommonUtil.ToDictionary(xml);
            if (!dic2.ContainsKey("appid"))
            {
                LogUtil.Warn("查找不到appid");
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("", Encoding.UTF8, "text/plain") };
            }

            string appId = dic2["appid"].ToString();

            var appInfo = AdminServiceFactory.AppInfo.Get(appId);

            if (!SdkFactory.Wx.CheckPayNotifySign(appInfo, xml))
            {
                LogUtil.Warn("支付通知结果签名验证失败");
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("", Encoding.UTF8, "text/plain") };
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
                string sb = "<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>";
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(sb, Encoding.UTF8, "text/plain") };
            }

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("", Encoding.UTF8, "text/plain") };

        }
    }
}