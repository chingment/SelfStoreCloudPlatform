﻿using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.MerchantMachine
{
    public class SearchCondition : BaseSearchCondition
    {
        public string DeviceId { get; set; }

        public int MerchantId { get; set; }
    }
}