using Lumos.Web.Http;
using Lumos;
using Lumos.Entity;
using Lumos.WeiXinSdk;
using Lumos.BLL;
using System.Web;
using Lumos.BLL.Service.Admin;
using Lumos.BLL.Biz;
using System.Configuration;

namespace WebAppApi
{
    [OwnApiAuthorize]
    public class OwnApiBaseController : BaseController
    {
        private OwnApiHttpResult _result = new OwnApiHttpResult();

        public OwnApiHttpResult Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public OwnApiBaseController()
        {
            LogUtil.SetTrackId();
        }

        public OwnApiHttpResponse ResponseResult(OwnApiHttpResult result)
        {
            return new OwnApiHttpResponse(result);
        }

        public OwnApiHttpResponse ResponseResult(ResultType resultType, ResultCode resultCode, string message = null, object data = null)
        {
            _result.Result = resultType;
            _result.Code = resultCode;
            _result.Message = message;
            _result.Data = data;
            return new OwnApiHttpResponse(_result);
        }

        public string CurrentUserId
        {
            get
            {
                return OwnApiRequest.GetCurrentUserId();
            }
        }

        public WxAppInfoConfig CurrentWxMpAppInfo
        {
            get
            {
                var appInfo = BizFactory.Merchant.GetWxMpAppInfoConfig(this.MerchantId);
                return appInfo;
            }
        }

        public string MerchantId
        {
            get
            {
                var merchantId = ConfigurationManager.AppSettings["custom:MerchantId"];
                return merchantId;
            }
        }
    }
}