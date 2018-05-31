using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common.Highcharts
{
    public class Highchart
    {

        public Highchart()
        {
            this.series = new List<Series>();
            this.xAxis = new XAxis();
        }

        public XAxis xAxis { get; set; }

        public List<Series> series
        {
            get;
            set;
        }
    }
}
