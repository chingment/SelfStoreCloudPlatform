using Lumos.Entity;
using Lumos.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Order
{
    public class SearchCondition : BaseSearchCondition
    {
        public string OrderSn { get; set; }

        public Enumeration.OrderStatus OrderStatus { get; set; }

        public string Nickname { get; set; }

    }
}