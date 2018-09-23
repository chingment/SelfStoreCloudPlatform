
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
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebAppApi.Controllers
{
    public class HomeController : Controller
    {
        private string key = "test";
        private string secret = "6ZB97cdVz211O08EKZ6yriAYrHXFBowC";
        private long timespan = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds;
        private string host = "";

        Dictionary<string, string> model = new Dictionary<string, string>();

        public ActionResult Index()
        {
            object isTest = ConfigurationManager.AppSettings["custom:IsTest"];
            if (isTest == null)
            {
                isTest = "false";
            }

            if (isTest.ToString() == "false")
            {
                host = "https://demo.res.17fanju.com";
            }
            else
            {
                host = "http://localhost:16665";
            }


            string userId = "00000000000000000000000000000000";
            string storeId = "be9ae32c554d4942be4a42fa48446210";



            //model.Add("获取全局数据", GlobalDataSet(userId, storeId, DateTime.Parse("2018-04-09 15:14:28")));
            //model.Add("获取全局数据", ShippingAddress(userId, storeId));
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

        //public string ShippingAddress(string userId, string storeId)
        //{
        //    Models.ShippingAddress.EditModel model = new Models.ShippingAddress.EditModel();

        //    model.UserId = userId;
        //    model.PhoneNumber = "15989287032";
        //    model.Address = "3123";
        //    model.AreaCode = "1";
        //    model.AreaName = "2";
        //    model.Consignee = "Sda";

        //    string a1 = JsonConvert.SerializeObject(model);

        //    string signStr = Signature.Compute(key, secret, timespan, a1);


        //    Dictionary<string, string> headers = new Dictionary<string, string>();
        //    headers.Add("key", key);
        //    headers.Add("timestamp", timespan.ToString());
        //    headers.Add("sign", signStr);
        //    headers.Add("version", "1.3.0.7");
        //    HttpUtil http = new HttpUtil();
        //    string result = http.HttpPostJson("" + host + "/api/ShippingAddress/Edit?userId=1&storeId=2", a1, headers);

        //    return result;

        //}

    }
}