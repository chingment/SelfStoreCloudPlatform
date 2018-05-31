using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Mvc
{
    public class BaseSearchCondition
    {
        public BaseSearchCondition()
        {
            this.PageSize = 10;
        }

        public string Sn { get; set; }

        public string Name { get; set; }

        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
