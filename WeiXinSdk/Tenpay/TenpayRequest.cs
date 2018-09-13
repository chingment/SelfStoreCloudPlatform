using Lumos.WeiXinSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using System.Xml;

namespace Lumos.WeiXinSdk.Tenpay
{
    public class TenpayRequest : ITenpayRequest
    {
        private string _returnCoennt;

        public string ReturnContent
        {
            get
            {
                return _returnCoennt;
            }
        }

        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string serverUrl = "https://api.mch.weixin.qq.com";



        private string GetServerUrl(string serverurl, string apiname)
        {
            return serverurl + "/" + apiname;
        }

        public SortedDictionary<string, string> DoPost(IWxConfig config, ITenpayPostApi request,bool isUserCert=false)
        {
            string realServerUrl = GetServerUrl(this.serverUrl, request.ApiName);

            WebUtils webUtils = new WebUtils();

            _returnCoennt = webUtils.DoPost(config, realServerUrl, request.PostData, isUserCert);

 
            SortedDictionary<string, string> m_values = new SortedDictionary<string, string>();


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(_returnCoennt);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                m_values[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }


            return m_values;
        }


    }
}
