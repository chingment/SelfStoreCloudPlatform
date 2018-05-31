using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.User
{
    public class SearchCondition : BaseSearchCondition
    {
        public string UserName { get; set; }
        public string FullName { get; set; }

    }
}