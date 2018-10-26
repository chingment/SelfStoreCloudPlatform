using System;


namespace Lumos.Web.Mvc
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
