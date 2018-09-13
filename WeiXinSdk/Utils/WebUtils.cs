using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Lumos.WeiXinSdk.Tenpay;

namespace Lumos.WeiXinSdk
{
    /// <summary>
    /// 网络工具类。
    /// </summary>
    public sealed class WebUtils
    {
        private int _timeout = 20000;
        private int _readWriteTimeout = 60000;
        private bool _ignoreSSLCheck = true;
        /// <summary>
        /// 等待请求开始返回的超时时间
        /// </summary>
        public int Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }

        /// <summary>
        /// 等待读取数据完成的超时时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return this._readWriteTimeout; }
            set { this._readWriteTimeout = value; }
        }

        /// <summary>
        /// 是否忽略SSL检查
        /// </summary>
        public bool IgnoreSSLCheck
        {
            get { return this._ignoreSSLCheck; }
            set { this._ignoreSSLCheck = value; }
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        public string DoPost(IWxConfig config, string url, string xml, bool isUseCert = false, int timeout = 1000)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";


                LogUtil.Info(string.Format("提交URL：{0},参数：{1}", url, xml));


                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                //是否使用证书
                if (isUseCert)
                {
                    LogUtil.Info("WxPayApi:path:" + config.SslCert_Path);
                    X509Certificate2 cer = new X509Certificate2(config.SslCert_Path, config.SslCert_Password, X509KeyStorageFlags.PersistKeySet);
                    request.ClientCertificates.Add(cer);
                    LogUtil.Info("WxPayApi:PostXml used cert");
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();

                LogUtil.Info(string.Format("提交URL：{0},返回结果：{1}", url, result));

                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                LogUtil.Error("HttpService:Thread - caught ThreadAbortException - resetting.");
                LogUtil.Error(string.Format("Exception message: {0}", e.Message));
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                LogUtil.Error(string.Format("HttpService:{0}", e.ToString()));
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    LogUtil.Error(string.Format("HttpService:StatusCode :{0} ", ((HttpWebResponse)e.Response).StatusCode));
                    LogUtil.Error(string.Format("HttpService:StatusDescription : {0}", ((HttpWebResponse)e.Response).StatusDescription));
                }

            }
            catch (Exception e)
            {
                LogUtil.Error(string.Format("HttpService:{0}", e.ToString()));
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }


        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <param name="headerParams">请求头部参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> urlParams, string postdata, IDictionary<string, string> headerParams = null)
        {

            if (urlParams != null && urlParams.Count > 0)
            {
                url = BuildRequestUrl(url, urlParams);
            }

            HttpWebRequest req = GetWebRequest(url, "POST", headerParams);
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";


            byte[] postData = Encoding.UTF8.GetBytes(postdata);
            System.IO.Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = GetResponseEncoding(rsp);
            return GetResponseAsString(rsp, encoding);
        }

        public string DoPost(string url, string postdata)
        {

            HttpWebRequest req = GetWebRequest(url, "POST", null);
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";


            byte[] postData = Encoding.UTF8.GetBytes(postdata);
            System.IO.Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = GetResponseEncoding(rsp);
            string result = GetResponseAsString(rsp, encoding);
            return result;
        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> textParams)
        {
            return DoGet(url, textParams, null);
        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <param name="headerParams">请求头部参数</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> textParams, IDictionary<string, string> headerParams)
        {
            if (textParams != null && textParams.Count > 0)
            {
                url = BuildRequestUrl(url, textParams);
            }
            LogUtil.Info(string.Format("请求url：{0}", url));
            HttpWebRequest req = GetWebRequest(url, "GET", headerParams);
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = GetResponseEncoding(rsp);
            return GetResponseAsString(rsp, Encoding.UTF8);
        }


        /// <summary>
        /// 执行带body体的POST请求。
        /// </summary>
        /// <param name="url">请求地址，含URL参数</param>
        /// <param name="body">请求body体字节流</param>
        /// <param name="contentType">body内容类型</param>
        /// <param name="headerParams">请求头部参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, byte[] body, string contentType, IDictionary<string, string> headerParams)
        {
            HttpWebRequest req = GetWebRequest(url, "POST", headerParams);
            req.ContentType = contentType;
            if (body != null)
            {
                System.IO.Stream reqStream = req.GetRequestStream();
                reqStream.Write(body, 0, body.Length);
                reqStream.Close();
            }
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = GetResponseEncoding(rsp);
            return GetResponseAsString(rsp, encoding);
        }

        public HttpWebRequest GetWebRequest(string url, string method, IDictionary<string, string> headerParams)
        {
            HttpWebRequest req = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                if (this._ignoreSSLCheck)
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(TrustAllValidationCallback);
                }
                req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
            }

            if (headerParams != null && headerParams.Count > 0)
            {
                foreach (string key in headerParams.Keys)
                {
                    req.Headers.Add(key, headerParams[key]);
                }
            }

            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "top-sdk-net";
            req.Accept = "text/xml,text/javascript";
            req.Timeout = this._timeout;
            req.ReadWriteTimeout = this._readWriteTimeout;

            return req;
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                if (Constants.CONTENT_ENCODING_GZIP.Equals(rsp.ContentEncoding, StringComparison.OrdinalIgnoreCase))
                {
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                }
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        /// <summary>
        /// 组装含参数的请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数映射</param>
        /// <returns>带参数的请求URL</returns>
        public static string BuildRequestUrl(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                return BuildRequestUrl(url, BuildQuery(parameters));
            }
            return url;
        }

        /// <summary>
        /// 组装含参数的请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queries">一个或多个经过URL编码后的请求参数串</param>
        /// <returns>带参数的请求URL</returns>
        public static string BuildRequestUrl(string url, params string[] queries)
        {
            if (queries == null || queries.Length == 0)
            {
                return url;
            }

            StringBuilder newUrl = new StringBuilder(url);
            bool hasQuery = url.Contains("?");
            bool hasPrepend = url.EndsWith("?") || url.EndsWith("&");

            foreach (string query in queries)
            {
                if (!string.IsNullOrEmpty(query))
                {
                    if (!hasPrepend)
                    {
                        if (hasQuery)
                        {
                            newUrl.Append("&");
                        }
                        else
                        {
                            newUrl.Append("?");
                            hasQuery = true;
                        }
                    }
                    newUrl.Append(query);
                    hasPrepend = false;
                }
            }
            return newUrl.ToString();
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return null;
            }

            StringBuilder query = new StringBuilder();
            bool hasParam = false;

            foreach (KeyValuePair<string, string> kv in parameters)
            {
                string name = kv.Key;
                string value = kv.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        query.Append("&");
                    }

                    query.Append(name);
                    query.Append("=");
                    query.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    hasParam = true;
                }
            }

            return query.ToString();
        }

        private Encoding GetResponseEncoding(HttpWebResponse rsp)
        {
            string charset = rsp.CharacterSet;
            if (string.IsNullOrEmpty(charset))
            {
                charset = Constants.CHARSET_UTF8;
            }
            return Encoding.GetEncoding(charset);
        }

        private static bool TrustAllValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; // 忽略SSL证书检查
        }
    }
}
