using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
  public  class RopProductKindEditSort
    {
        public List<Dic> Dics { get; set; }

        public class Dic
        {
            public string Id { get; set; }
            public int Priority { get; set; }
        }
    }
}
