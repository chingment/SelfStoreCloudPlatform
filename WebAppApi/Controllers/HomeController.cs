
using log4net;
using Lumos.BLL;
using Lumos.Common;
using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAppApi.Models;
using WebAppApi.Models.Account;
using WebAppApi.Models.Order;

namespace WebAppApi.Controllers
{
    public class HomeController : Controller
    {
        private string key = "test";
        private string secret = "6ZB97cdVz211O08EKZ6yriAYrHXFBowC";
        private long timespan = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds;
        private string host = "http://localhost:16665";
        //private string host = "https://demo.gzhaoyilian.com";
        // private string host = "http://api.gzhaoyilian.com";
        // private string host = "https://www.ins-uplink.cn";
        //private string host = "http://120.79.233.231";


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

        public string BulidOpendId(string merchantCode, string key, string secret)
        {

            byte[] result = Encoding.Default.GetBytes(merchantCode + key + secret);    //tbPass为输入密码的文本框  
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string s_output = BitConverter.ToString(output).Replace("-", "");

            return s_output;
        }

        private decimal GetDecimal(decimal d)
        {
            return Math.Round(d, 2);
        }

        Dictionary<string, string> model = new Dictionary<string, string>();

        public ActionResult Index()
        {

            string userId = "00000000000000000000000000000000";
            string storeId = "be9ae32c554d4942be4a42fa48446210";



            model.Add("获取全局数据", GlobalDataSet(userId, storeId, DateTime.Parse("2018-04-09 15:14:28")));

            //model.Add("获取地址", GetShippingAddress(1215));

            return View(model);
        }

        public static string stringSort(string str)
        {
            char[] chars = str.ToCharArray();
            List<string> lists = new List<string>();
            foreach (char s in chars)
            {
                lists.Add(s.ToString());
            }
            lists.Sort();//sort默认是从小到大的。显示123456789      

            str = "";
            foreach (string item in lists)
            {
                str += item;
            }
            return str;
        }

        public string GlobalDataSet(string userId, string storeId, DateTime datetime)
        {
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("userId", userId);
            parames.Add("storeId", storeId.ToString());
            parames.Add("datetime", datetime.ToUnifiedFormatDateTime());
            string signStr = Signature.Compute(key, secret, timespan, Signature.GetQueryData(parames));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("key", key);
            headers.Add("timestamp", timespan.ToString());
            headers.Add("sign", signStr);
            HttpUtil http = new HttpUtil();
            string result = http.HttpGet("" + host + "/api/Global/DataSet?userId=" + userId + "&storeId=" + storeId + "&datetime=" + HttpUtility.UrlEncode(datetime.ToUnifiedFormatDateTime(), UTF8Encoding.UTF8).ToUpper(), headers);

            return result;

        }

    }
}