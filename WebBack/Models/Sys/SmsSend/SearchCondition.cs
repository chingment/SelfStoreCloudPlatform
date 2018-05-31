using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.SmsSend
{
    public class SearchCondition : BaseSearchCondition
    {
        public string Phone { get; set; }
    }
}