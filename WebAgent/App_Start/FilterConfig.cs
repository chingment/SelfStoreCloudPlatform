using System.Web;
using System.Web.Mvc;
using WebAgent.Controllers;

namespace WebAgent
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //监控引用
            filters.Add(new OwnStatisticsTrackerAttribute());
        }
    }
}
