using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common.Highcharts
{
    public class XAxis
    {
        public XAxis()
        {
            this.categories = new List<string>();
        }

        public List<string> categories { get; set; }
    }
}
