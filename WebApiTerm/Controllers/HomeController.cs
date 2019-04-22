
using Lumos.BLL.Service.ApiTerm;
using Lumos.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApiTerm.Controllers
{
    public class HomeController : Controller
    {
        private string key = "test";
        private string secret = "6ZB97cdVz211O08EKZ6yriAYrHXFBowC";
        private long timespan = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds;

        private string host = "";

        public static string GetQueryString(Dictionary<string, string> parames)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parames);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");  //签名字符串
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (parames == null || parames.Count == 0)
                return "";

            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }

            string s = queryStr.ToString().Substring(1, queryStr.Length - 1);

            return s;
        }


        public static string GetQueryString2(Dictionary<string, string> parames)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parames);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");  //签名字符串
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (parames == null || parames.Count == 0)
                return "";

            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }



            string s = queryStr.ToString().Substring(1, queryStr.Length - 1);

            return s;
        }

        private decimal GetDecimal(decimal d)
        {
            return Math.Round(d, 2);
        }

        Dictionary<string, string> model = new Dictionary<string, string>();

        public ActionResult Index()
        {
            if (ConfigurationManager.AppSettings["custom:IsTest"] == null)
            {
                host = "http://localhost:18665/";
            }
            else
            {
                host = "http://demo.api.term.17fanju.com";
            }

            //host = "http://demo.api.term.17fanju.com";

            string machineId = "000000000000000";
            //model.Add("获取机器初始数据", MachineInitData(machineId));
            //model.Add("预定商品", OrderReserve(machineId));
            model.Add("生成支付二维码", OrderPayUrlBuild(machineId, "bc427a99e2e94e338f9bcea16d67a062"));

            //model.Add("登陆机器", MachineLogin(machineId,"a","b"));

            //HttpUtil http = new HttpUtil();

            //http.HttpUploadFile(host+ "/Api/Machine/UpLoadLog", "d:\\a.txt");

            return View(model);
        }



        public string MachineInitData(string machineId)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("machineId", machineId.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Machine/InitData?machineId=" + machineId, headers);

            return result;

        }

        public string OrderReserve(string machineId)
        {

            RopOrderReserve pms = new RopOrderReserve();
            pms.MachineId = machineId;
            pms.Skus.Add(new RopOrderReserve.Sku() { Id = "3cbddffaf84148279bd91551db238ca3", Quantity = 1 });
            pms.Skus.Add(new RopOrderReserve.Sku() { Id = "44b2d4ae88e24b76a8a744f582214513", Quantity = 1 });
            pms.Skus.Add(new RopOrderReserve.Sku() { Id = "e8bb8685ed8d483fa60225fc750f3a79", Quantity = 1 });


            string a1 = JsonConvert.SerializeObject(pms);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Order/Reserve", a1, headers1);

            return respon_data4;

        }

        public string OrderPayUrlBuild(string machineId, string orderId)
        {

            RopOrderPayUrlBuild pms = new RopOrderPayUrlBuild();
            pms.MachineId = machineId;
            pms.PayWay = PayWay.Wechat;
            pms.OrderId = orderId;


            string a1 = JsonConvert.SerializeObject(pms);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Order/PayUrlBuild", a1, headers1);

            return respon_data4;

        }

        public string MachineLogin(string machineId, string userName, string password)
        {

            RopMachineLogin pms = new RopMachineLogin();
            pms.MachineId = machineId;
            pms.UserName = userName;
            pms.Password = password;


            string a1 = JsonConvert.SerializeObject(pms);

            string signStr = Signature.Compute(key, secret, timespan, a1);

            Dictionary<string, string> headers1 = new Dictionary<string, string>();
            headers1.Add("key", key);
            headers1.Add("timestamp", (timespan.ToString()).ToString());
            headers1.Add("sign", signStr);

            HttpUtil http = new HttpUtil();
            string respon_data4 = http.HttpPostJson("" + host + "/api/Machine/Login", a1, headers1);

            return respon_data4;

        }
    }
}