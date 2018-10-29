using Lumos.Web.Http;
using Lumos;
using Lumos.Entity;
using Lumos.WeiXinSdk;
using Lumos.BLL;
using System.Web;

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

        public AppInfoConfig CurrentAppInfo
        {
            get
            {
                var context = HttpContext.Current;
                var request = context.Request;
                var appId = request.Params["AppId"];
                if (appId == null)
                    return null;
                var app = SysFactory.AppInfo.Get(appId);
                var appInfo = new AppInfoConfig();
                appInfo.AppId = app.AppId;
                appInfo.AppSecret = app.AppSecret;
                return appInfo;
            }
        }
    }
}