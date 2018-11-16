using System.Web;
using System.Web.Mvc;
using WebMerch.Controllers;

namespace WebMerch
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
