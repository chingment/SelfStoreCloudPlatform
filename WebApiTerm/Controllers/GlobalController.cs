using Lumos;
using Lumos.BLL.Service.AppTerm;
using System.Web;
using System.Web.Http;
namespace WebApiTerm.Controllers
{
    [OwnApiAuthorize]
    public class GlobalController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse DataSet([FromUri]RupGlobalDataSet rup)
        {
            IResult result = TermServiceFactory.Global.DataSet(rup);
            return new OwnApiHttpResponse(result);
        }

    }
}