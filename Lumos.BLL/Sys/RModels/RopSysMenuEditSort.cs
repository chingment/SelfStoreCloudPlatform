using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Sys
{
    public class RopSysMenuEditSort
    {
        public List<Dic> Dics { get; set; }

        public class Dic
        {
            public string MenuId { get; set; }
            public int Priority { get; set; }
        }
    }
}
