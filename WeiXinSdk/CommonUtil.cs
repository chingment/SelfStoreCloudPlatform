using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lumos.WeiXinSdk
{
    public static class CommonUtil
    {
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string GetNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string MakeMd5Sign(SortedDictionary<string, object> m_values, string key = null)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {

                }

                if (pair.Key != "sign" && pair.Value != null && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');

            //转url格式
            string str = buff;
            //在string后加入API KEY
            if (!string.IsNullOrEmpty(key))
            {
                str += "&key=" + key;
            }
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        //创建sha1签名
        public static string MakeSHA1Sign(SortedDictionary<string, string> parameters)
        {
            StringBuilder sb = new StringBuilder();
            ArrayList akeys = new ArrayList(parameters.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(k + "=" + v);
                    }
                    else
                    {
                        sb.Append("&" + k + "=" + v);
                    }
                }
            }

            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();
            //将mystr转换成byte[] 
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(sb.ToString());
            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            string paySign = hash.ToLower();

            return paySign;
        }


        public static string MakeCardSign(Dictionary<string, string> parameters)
        {
            StringBuilder sb = new StringBuilder();


            var vDic = parameters.OrderBy(o => o.Value);


            foreach (KeyValuePair<string, string> k in vDic)
            {
                sb.Append(k.Value);
            }


            //var vDic = (from objDic in parameters orderby objDic.Value ascending select objDic);

            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();
            //将mystr转换成byte[] 
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(sb.ToString());
            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            string paySign = hash.ToLower();

            return paySign;
        }

        public static SortedDictionary<string, object> ToDictionary(string xml)
        {
            SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                m_values[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }

            if (m_values["return_code"] != null)
            {
                if (m_values["return_code"].ToString() != "SUCCESS")
                {
                    return m_values;
                }
            }

            return m_values;
        }

    }
}
