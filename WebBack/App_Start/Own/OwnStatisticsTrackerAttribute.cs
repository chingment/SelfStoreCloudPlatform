﻿using System;
using Lumos.Web.Mvc;

namespace WebBack
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class OwnStatisticsTrackerAttribute : BaseStatisticsTrackerAttribute
    {

    }

}