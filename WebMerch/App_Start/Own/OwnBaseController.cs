using System.Web.Mvc;
using Lumos.Web.Mvc;
using Lumos.DAL;
using System.Linq;

namespace WebMerch
{

    /// <summary>
    /// BaseController用来扩展Controller,凡是在都该继承BaseController
    /// </summary>

    [OwnAuthorize]
    [ValidateInput(false)]
    public abstract class OwnBaseController : BaseController
    {
        private string _merchantId = "";

        public override string CurrentUserId
        {
            get
            {
                return OwnRequest.GetCurrentUserId();
            }
        }

        public string CurrentMerchantId
        {
            get
            {
                return OwnRequest.GetCurrentMerchantId();
            }
        }

        public OwnBaseController()
        {


        }



        private LumosDbContext _currentDb;

        public LumosDbContext CurrentDb
        {
            get
            {
                if (_currentDb == null)
                {
                    _currentDb = new LumosDbContext();
                }

                return _currentDb;
            }
        }
    }
}