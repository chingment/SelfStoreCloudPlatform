using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lumos.Entity;
using Lumos.Web.Mvc;

namespace WebMerch.Models.ProductSku
{
    public class SearchCondition : BaseSearchCondition
    {
        public string Id { get; set; }
    }
}