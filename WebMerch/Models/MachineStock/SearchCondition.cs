using Lumos.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.MachineStock
{
    public class SearchCondition : BaseSearchCondition
    {
        public string StoreId { get; set; }

        public string MerchantId { get; set; }

        public string MachineId { get; set; }

    }
}