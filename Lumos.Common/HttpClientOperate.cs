using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using log4net;
namespace Lumos.Common
{
    public static class HttpClientOperate
    {

        public static T Post<T>(string requestUri, string webapiBaseUrl, HttpContent httpContent)
        {
            ILog log = LogManager.GetLogger("");

            var httpClient = new HttpClient()

            {

                MaxResponseContentBufferSize = 1024 * 1024 * 2,

                BaseAddress = new Uri(webapiBaseUrl)

            };



            T t = Activator.CreateInstance<T>();



            HttpResponseMessage response = new HttpResponseMessage();



            try
            {

                httpClient.PostAsync(webapiBaseUrl, httpContent).ContinueWith((task) =>
                {

                    if (task.Status != TaskStatus.Canceled)
                    {

                        response = task.Result;

                    }

                }).Wait(100000);



                if (response.Content != null && response.StatusCode == HttpStatusCode.OK)
                {

                    t = response.Content.ReadAsAsync<T>().Result;

                }

                return t;

            }

            catch (Exception ex)
            {
                log.Error(ex);
                return t;

            }

            finally
            {

                httpClient.Dispose();

                response.Dispose();

            }

        }





        public static T Post<T>(string requestUri, string webapiBaseUrl, string jsonString)
        {

            HttpContent httpContent = new StringContent(jsonString);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Post<T>(requestUri, webapiBaseUrl, httpContent);

        }





        public static T Post<T>(string requestUri, string webapiBaseUrl, object obj = null)
        {

            string jsonString = JsonConvert.SerializeObject(obj);//可换成Newtonsoft.Json的JsonConvert.SerializeObject方法将对象转化为json字符串

            return Post<T>(requestUri, webapiBaseUrl, jsonString);

        }
    }
}
