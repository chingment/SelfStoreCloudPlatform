using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos
{
    /// <summary>
    /// 监控日志对象
    /// </summary>
    public class MonitorLog
    {
        public string ControllerName
        {
            get;
            set;
        }
        public string ActionName
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public string RequestUrl
        {
            get;
            set;
        }

        public DateTime RequestTime
        {
            get;
            set;
        }
        public DateTime ResponseTime
        {
            get;
            set;
        }
        /// <summary>
        /// Form 表单数据
        /// </summary>
        public NameValueCollection FormCollections
        {
            get;
            set;
        }
        /// <summary>
        /// URL 参数
        /// </summary>
        public NameValueCollection QueryCollections
        {
            get;
            set;
        }

        public string ResponseData
        {
            get;
            set;
        }


        /// <summary>
        /// 监控类型
        /// </summary>
        public enum MonitorType
        {
            Action = 1,
            View = 2
        }
        /// <summary>
        /// 获取监控指标日志
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public string GetRequestInfo()
        {
            string Msg = @"
      ControllerName：{0},ActionName:{1}
     请求Url：{2}
      请求时间：{3}
      用户Id:{4}
      Form表单数据：{5}
      URL参数：{6}
          ";
            return string.Format(Msg,
              ControllerName,
              ActionName,
              RequestUrl,
              RequestTime,
              UserId,
              GetCollections(FormCollections),
              GetCollections(QueryCollections));
        }


        public string GetResponseInfo()
        {
            string Msg = @"
      ControllerName：{0}Controller,ActionName:{1}
      请求时间：{2}
      请求Url：{3}
      用户Id:{4}
      Form表单数据：{5}
      URL参数：{6}
      响应时间：{7}
      总 时 间：{8}秒
      响应文本：{9}
          ";
            return string.Format(Msg,
              ControllerName,
              ActionName,
              RequestTime,
              RequestUrl,
              UserId,
              GetCollections(FormCollections),
              GetCollections(QueryCollections),
              ResponseTime,
              (ResponseTime - RequestTime).TotalSeconds,
              ResponseData
              );
        }

        /// <summary>
        /// 获取Post 或Get 参数
        /// </summary>
        /// <param name="Collections"></param>
        /// <returns></returns>
        public string GetCollections(NameValueCollection Collections)
        {
            string Parameters = string.Empty;
            if (Collections == null || Collections.Count == 0)
            {
                return Parameters;
            }
            foreach (string key in Collections.Keys)
            {
                if (key != null)
                {
                    string k = key.ToLower();
                    if (k.IndexOf("password") == -1 && k.IndexOf("pwd") == -1)
                    {


                        Parameters += string.Format("{0}={1}&", key, Collections[key]);
                    }
                    else
                    {
                        Parameters += string.Format("{0}={1}&", key, "");
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(Parameters) && Parameters.EndsWith("&"))
            {
                Parameters = Parameters.Substring(0, Parameters.Length - 1);
            }
            return Parameters;
        }
    }

}
