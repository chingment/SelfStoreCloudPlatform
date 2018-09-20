using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models
{
    public class InsPlan
    {
        public List<InsPlanCol> Cols { get; set; }

        public List<InsPlanRow> Rows { get; set; }
    }

    public class InsPlanCol
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class InsPlanRow
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<InsPlanRowVal> Vals { get; set; }
    }

    public class InsPlanRowVal
    {
        public int ColId { get; set; }

        public string Title { get; set; }
    }


}