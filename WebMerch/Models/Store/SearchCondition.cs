﻿using Lumos.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Store
{
    public class SearchCondition : BaseSearchCondition
    {
        public string StoreId { get; set; }
    }
}