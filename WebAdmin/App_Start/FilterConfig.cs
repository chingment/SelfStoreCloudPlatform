using System.Web;
using System.Web.Mvc;
using WebAdmin.Controllers;

namespace WebAdmin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //监控引用
            filters.Add(new OwnExceptionAttribute());
            filters.Add(new OwnStatisticsTrackerAttribute());
        }
    }
}
