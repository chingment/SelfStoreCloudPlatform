using System;
using Lumos.Web.Mvc;

namespace WebMobile
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class OwnStatisticsTrackerAttribute : BaseStatisticsTrackerAttribute
    {

    }

}