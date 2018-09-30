
using log4net;
using Lumos.BLL;
using Lumos.BLL.Service.Term;
using Lumos.BLL.Service.Term.Models;
using Lumos.Common;
using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebTermApi.Controllers
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
                host = "http://localhost:18665";
            }
            else
            {
                host = "http://demo.api.term.17fanju.com";
            }


            string clientId = "ca66ca85c5bf435581ecd2380554ecf1";
            string merchantId = "d1e8ad564c0f4516b2de95655a4146c7";
            string machineId = "00000000000000000000000000000001";
            string storeId = "21ae9399b1804dbc9ddd3c29e8b5c670";
            //model.Add("获取机器接口配置信息", MachineApiConfig("000000000000000"));
            //model.Add("获取全局数据", GlobalDataSet(userId, merchantId, machineId, DateTime.Now));

            model.Add("预定商品", OrderReserve(merchantId, storeId, machineId));
            return View(model);
        }



        public string MachineApiConfig(string deviceId)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("deviceId", deviceId.ToString());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Machine/ApiConfig?deviceId=" + deviceId, headers);

            return result;

        }

        public string GlobalDataSet(string userId, string merchantId, string machineId, DateTime datetime)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId.ToString());
            parames.Add("merchantId", merchantId.ToString());
            parames.Add("machineId", machineId.ToString());
            parames.Add("datetime", datetime.ToUnifiedFormatDateTime());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Global/DataSet?userId=" + userId + "&merchantId=" + merchantId + "&machineId=" + machineId + "&datetime=" + HttpUtility.UrlEncode(datetime.ToUnifiedFormatDateTime(), UTF8Encoding.UTF8).ToUpper(), headers);

            return result;

        }

        public string OrderReserve(string merchantId, string storeId, string machineId)
        {

            RopOrderReserve pms = new RopOrderReserve();
            pms.MerchantId = merchantId;
            pms.StoreId = storeId;
            pms.MachineId = machineId;
            pms.PayWay = "";

            pms.Details.Add(new RopOrderReserve.Detail() { SkuId = "1", Quantity = 1 });
            pms.Details.Add(new RopOrderReserve.Detail() { SkuId = "2", Quantity = 1 });
            pms.Details.Add(new RopOrderReserve.Detail() { SkuId = "3", Quantity = 1 });
            pms.Details.Add(new RopOrderReserve.Detail() { SkuId = "4", Quantity = 1 });

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
    }
}