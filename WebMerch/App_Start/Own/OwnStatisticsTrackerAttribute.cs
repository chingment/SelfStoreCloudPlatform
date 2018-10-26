using System;
using Lumos.Web.Mvc;

namespace WebMerch
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class OwnStatisticsTrackerAttribute : BaseStatisticsTrackerAttribute
    {

    }

}