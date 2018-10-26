﻿using Lumos.Entity;
using Lumos.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.MerchantMachine
{
    public class SearchCondition : BaseSearchCondition
    {
        public string DeviceId { get; set; }

        public string MerchantId { get; set; }
    }
}