using System.Web.Mvc;
using Lumos.Web.Mvc;

namespace WebBack
{

    /// <summary>
    /// BaseController用来扩展Controller,凡是在都该继承BaseController
    /// </summary>
    [OwnException]
    [OwnAuthorize]
    [ValidateInput(false)]
    public abstract class OwnBaseController : BaseController
    {

        public override string CurrentUserId
        {
            get
            {
                return OwnRequest.GetCurrentUserId();
            }
        }
    }
}