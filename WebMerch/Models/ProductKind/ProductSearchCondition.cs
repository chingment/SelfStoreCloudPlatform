using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.ProductKind
{
    public class ProductSearchCondition: BaseSearchCondition
    {
        public string KindId { get; set; }
    }
}