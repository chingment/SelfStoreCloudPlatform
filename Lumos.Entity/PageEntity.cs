using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class PageEntity
    {
        public string Name { get; set; }

        public int TotalRecord { get; set; }

        public int PageSize { get; set; }

        public object Rows { get; set; }

        public object Status { get; set; }
    }
}