using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Machine
{
    public class SearchCondition : BaseSearchCondition
    {
        public string MacAddress { get; set; }

        public string DeviceId { get; set; }
    }
}