using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common.Highcharts
{
    public class Series
    {
        public Series()
        {
            this.data = new List<decimal?>();
        }
        public string name { get; set; }

        public List<decimal?> data { get; set; }
    }
}
