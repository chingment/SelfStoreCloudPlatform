using System;
using Lumos.Web.Mvc;

namespace WebAdmin
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class OwnStatisticsTrackerAttribute : BaseStatisticsTrackerAttribute
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