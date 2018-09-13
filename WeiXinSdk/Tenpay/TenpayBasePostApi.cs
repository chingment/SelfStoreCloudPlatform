using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.Tenpay
{
    public abstract class TenpayBasePostApi
    {
        /**
    * @生成签名，详见签名生成算法
    * @return 签名, sign字段不参加签名
    */
        public string MakeSign(SortedDictionary<string, object> m_values, string key)
        {
            return CommonUtil.MakeMd5Sign(m_values, key);
        }


        /**
* @将Dictionary转成xml
* @return 经转换得到的xml串
* @throws WxPayException
**/
        public virtual string GetXml(SortedDictionary<string, object> m_values)
        {
            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {

                }

                if (pair.Value!=null&&pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value != null && pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {

                }
            }
            xml += "</xml>";
            return xml;
        }
    }
}
