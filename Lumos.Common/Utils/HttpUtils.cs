using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common
{
    public class HttpUtil
    {
        public string HttpPost(string urlString, string body = "", string safeAccount = "", string safeAccountPwd = "")
        {
            string result = "";
            StringBuilder strLog = new StringBuilder();
            strLog.Append("\r\n");
            strLog.Append("[Function]:HttpPostJson\r\n");
            strLog.Append("[RequestURL]:" + urlString + "\r\n");
            strLog.Append("[Post]:\r\n");
            strLog.Append("" + body + "\r\n");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            HttpWebRequest webRequest = WebRequest.Create(urlString) as HttpWebRequest;
            if (!string.IsNullOrWhiteSpace(safeAccount))
            {
                //注意这里的格式哦，为 "username:password"
                string usernamePassword = safeAccount + ":" + safeAccountPwd;
                CredentialCache mycache = new CredentialCache();
                mycache.Add(new Uri(urlString), "Basic", new NetworkCredential(safeAccount, safeAccountPwd));
                webRequest.Credentials = mycache;
                webRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));
            }
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";
            webRequest.Accept = "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            webRequest.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            webRequest.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            webRequest.Headers.Add("Accept-Charset", "utf-8,GBK;q=0.7,*;q=0.3");

            byte[] bytes = Encoding.ASCII.GetBytes(body);
            Stream stream = null;
            try
            {
                webRequest.ContentLength = bytes.Length;
                stream = webRequest.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);         //Send it
                WebResponse webResponse = webRequest.GetResponse();


                stream = webResponse.GetResponseStream();

                var streamReader = new StreamReader(stream, Encoding.UTF8);
                if (webResponse.Headers["Content-Encoding"] != null && webResponse.Headers["Content-Encoding"].Equals("gzip"))
                {
                    var gzipStream = new GZipStream(stream, CompressionMode.Decompress);
                    streamReader = new StreamReader(gzipStream);
                }
                result = streamReader.ReadToEnd().Trim();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                sw.Stop();
                strLog.Append("[RequestTime]:" + sw.ElapsedMilliseconds + "ms\r\n");
                strLog.Append("[Return]:" + result + "\r\n");
                strLog.Append(" ----------------------------------------------------\r\n");


            }
            return result;
        }

        public string HttpPostJson(string urlString, string body, Dictionary<string, string> headers = null)
        {



            byte[] bytes = Encoding.UTF8.GetBytes(body);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString);

            //X509Certificate cer = new X509Certificate("D:\\ca\\intermediate2.cer");
            //request.ClientCertificates.Add(cer);


            if (headers != null)
            {
                foreach (var m in headers)
                {
                    request.Headers.Add(m.Key, m.Value);
                }
            }

            //写数据
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "application/json";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            //读数据
            request.Timeout = 300000;
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
            string strResult = streamReader.ReadToEnd();

            //关闭流
            reqstream.Close();
            streamReader.Close();
            streamReceive.Close();
            request.Abort();
            response.Close();


            return strResult;













            //string result = "";
            //StringBuilder strLog = new StringBuilder();
            //strLog.Append("\r\n");
            //strLog.Append("[Function]:HttpPostJson\r\n");
            //strLog.Append("[RequestURL]:" + urlString + "\r\n");
            //strLog.Append("[Post]:\r\n");
            //strLog.Append("" + body + "\r\n");
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //HttpWebRequest webRequest = WebRequest.Create(urlString) as HttpWebRequest;

            //// webRequest.ContentType = "application/json";
            //webRequest.ContentType = "application/json";
            //webRequest.Method = "POST";
            //webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";
            //webRequest.Accept = "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            //webRequest.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            //webRequest.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            //webRequest.Headers.Add("Accept-Charset", "utf-8,GBK;q=0.7,*;q=0.3");

            //if (headers != null)
            //{
            //    foreach (var m in headers)
            //    {
            //        webRequest.Headers.Add(m.Key, m.Value);
            //    }
            //}

            //byte[] bytes = Encoding.UTF8.GetBytes(body);
            //Stream stream = null;
            //try
            //{ //读数据
            //    webRequest.Timeout = 300000;
            //    webRequest.ContentLength = bytes.Length;
            //    stream = webRequest.GetRequestStream();
            //    stream.Write(bytes, 0, bytes.Length);         //Send it
            //    WebResponse webResponse = webRequest.GetResponse();


            //    stream = webResponse.GetResponseStream();

            //    var streamReader = new StreamReader(stream, Encoding.UTF8);
            //    if (webResponse.Headers["Content-Encoding"] != null && webResponse.Headers["Content-Encoding"].Equals("gzip"))
            //    {
            //        var gzipStream = new GZipStream(stream, CompressionMode.Decompress);
            //        streamReader = new StreamReader(gzipStream);
            //    }
            //    result = streamReader.ReadToEnd().Trim();
            //}
            //catch (Exception ex)
            //{

            //}
            //finally
            //{
            //    if (stream != null)
            //    {
            //        stream.Close();
            //    }
            //    sw.Stop();
            //    strLog.Append("[RequestTime]:" + sw.ElapsedMilliseconds + "ms\r\n");
            //    strLog.Append("[Return]:" + result + "\r\n");
            //    strLog.Append(" ----------------------------------------------------\r\n");


            //}
            //return result;
        }

        /// <summary>
        /// 通过GET方式获取页面的方法
        /// </summary>
        /// <param name="urlString">请求的URL</param>
        /// <param name="encoding">页面编码</param>
        /// <returns></returns>
        public string HttpGet(string urlString, Dictionary<string, string> headers = null)
        {
            //定义局部变量
            HttpWebRequest httpWebRequest;
            HttpWebResponse httpWebRespones;
            Stream stream;
            string result = "";
            StringBuilder strLog = new StringBuilder();
            strLog.Append("\r\n");
            strLog.Append("[Function]:HttpGet\r\n");
            strLog.Append("[RequestURL]:" + urlString + "\r\n");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            #region 请求页面
            try
            {
                httpWebRequest = WebRequest.Create(urlString) as HttpWebRequest;



                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";
                httpWebRequest.Accept = "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";


                httpWebRequest.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
                httpWebRequest.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
                httpWebRequest.Headers.Add("Accept-Charset", "utf-8,GBK;q=0.7,*;q=0.3");


                if (headers != null)
                {
                    foreach (var m in headers)
                    {
                        httpWebRequest.Headers.Add(m.Key, m.Value);
                    }
                }


                httpWebRespones = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebRespones.GetResponseStream();

                var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8);
                if (httpWebRespones.Headers["Content-Encoding"] != null && httpWebRespones.Headers["Content-Encoding"].Equals("gzip"))
                {
                    var gzipStream = new GZipStream(stream, CompressionMode.Decompress);
                    streamReader = new StreamReader(gzipStream);
                }

                result = streamReader.ReadToEnd();
                if (streamReader != null)
                {
                    streamReader.Close();
                    stream.Close();
                }
            }
            //处理异常
            catch (System.Exception ex)
            {

            }
            //释放资源返回结果
            finally
            {
                sw.Stop();
                strLog.Append("[RequestTime]:" + sw.ElapsedMilliseconds + "ms\r\n");
                strLog.Append("[Return]:" + result + "\r\n");
                strLog.Append(" ----------------------------------------------------\r\n");

            }
            #endregion
            return result;
        }
    }
}
