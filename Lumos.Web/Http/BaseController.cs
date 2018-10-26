using Newtonsoft.Json;
using System.Web.Http;

namespace Lumos.Web.Http
{
    public abstract class BaseController : ApiController
    {
        #region JsonResult 扩展

        protected CustomJsonResult Json(string contenttype, ResultType type, object content, string message, params JsonConverter[] converters)
        {
            return new CustomJsonResult(contenttype, type, message, content);
        }

        protected CustomJsonResult Json(ResultType type, object content, string message, params JsonConverter[] converters)
        {
            return new CustomJsonResult(null, type, message, content, converters);

        }

        protected internal CustomJsonResult Json(ResultType type)
        {
            return Json(type, null, null);
        }

        protected internal CustomJsonResult Json(ResultType type, string message)
        {
            return Json(type, null, message);
        }

        protected internal CustomJsonResult Json(string contenttype, ResultType type, string message)
        {
            return Json(contenttype, type, null, message);
        }

        protected internal CustomJsonResult Json(ResultType type, object content)
        {
            return Json(type, content, null);
        }
        #endregion
    }
}
