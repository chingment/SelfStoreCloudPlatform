using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using log4net;
using System.Reflection;

namespace Lumos.WeiXinSdk
{

    public interface IWxApi
    {
        T DoGet<T>(IWxApiGetRequest<T> request) where T : WxApiBaseResult;

        T DoPost<T>(IWxApiPostRequest<T> request) where T : WxApiBaseResult;
    }

    public class WxApi : IWxApi
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string responseString = null;
        public WxApi()
        {

        }

        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }

        public T DoGet<T>(IWxApiGetRequest<T> request) where T : WxApiBaseResult
        {
            string realServerUrl = request.ApiUrl;
            WebUtils webUtils = new WebUtils();
            string body = webUtils.DoGet(realServerUrl, request.GetUrlParameters(), null);
            log.InfoFormat("WeiXinSdk-Get->{0}", body);
            T rsp = JsonConvert.DeserializeObject<T>(body);


            return rsp;
        }

        public T DoPost<T>(IWxApiPostRequest<T> request) where T : WxApiBaseResult
        {
            // request.GetUrlParameters()

            string realServerUrl = request.ApiUrl;

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

            log.InfoFormat("WeiXinSdk-Post->{0}", postData);
            responseString = webUtils.DoPost(realServerUrl, request.GetUrlParameters(), postData);
            log.InfoFormat("WeiXinSdk-Result->{0}", responseString);
            T rsp = JsonConvert.DeserializeObject<T>(responseString);


            return rsp;
        }


    }
}
