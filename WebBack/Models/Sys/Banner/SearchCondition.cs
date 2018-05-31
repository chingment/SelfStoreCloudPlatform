using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.Banner
{
    public class SearchCondition:BaseSearchCondition
    {
        public string Title { get; set; }

        public Enumeration.BannerType Type{ get; set; }
    }
}