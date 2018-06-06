using Lumos.Mvc;
using System.Web;
using System.Web.Mvc;
using WebSSO.Controllers;

namespace WebSSO
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
             filters.Add(new OwnExceptionAttribute());
             filters.Add(new OwnStatisticsTrackerAttribute());
        }
    }
}
