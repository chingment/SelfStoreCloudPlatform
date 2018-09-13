using log4net;
using Lumos.Common;
using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Lumos;
using System.Reflection;
using Lumos.Mvc;

namespace WebMobile
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class OwnStatisticsTrackerAttribute : BaseStatisticsTrackerAttribute
    {

    }

}