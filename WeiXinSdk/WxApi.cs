using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Lumos.WeiXinSdk
{

    public interface IWxApi
    {
        T DoGet<T>(IWxApiGetRequest<T> request) where T : WxApiBaseResult;

        T DoPost<T>(IWxApiPostRequest<T> request) where T : WxApiBaseResult;
    }

    public class WxApi : IWxApi
    {
        public string serverUrl = "https://api.weixin.qq.com";


        public WxApi()
        {

        }

        public string GetServerUrl(string serverurl, string apiname)
        {
            return serverurl + "/" + apiname;
        }

        public T DoGet<T>(IWxApiGetRequest<T> request) where T : WxApiBaseResult
        {
            string realServerUrl = GetServerUrl(this.serverUrl, request.ApiName);
            WebUtils webUtils = new WebUtils();
            string body = webUtils.DoGet(realServerUrl, request.GetUrlParameters(), null);
            T rsp = JsonConvert.DeserializeObject<T>(body);


            return rsp;
        }


        public T DoPost<T>(IWxApiPostRequest<T> request) where T : WxApiBaseResult
        {

            string realServerUrl = GetServerUrl(this.serverUrl, request.ApiName);
            WebUtils webUtils = new WebUtils();

            string postData = null;
            if (request.PostDataTpye == WxPostDataType.Text)
            {
                postData = request.PostData.ToString();
            }
            else if (request.PostDataTpye == WxPostDataType.Json)
            {
                postData = JsonConvert.SerializeObject(request.PostData);
            }


            string body = webUtils.DoPost(realServerUrl, request.GetUrlParameters(), postData, null);
            T rsp = JsonConvert.DeserializeObject<T>(body);


            return rsp;
        }


    }
}
