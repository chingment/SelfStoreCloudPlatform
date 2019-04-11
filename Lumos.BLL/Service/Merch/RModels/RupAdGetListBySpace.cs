using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RupAdGetListBySpace : RupBaseGetList
    {
        public Enumeration.AdSpaceBelong Belong { get; set; }
    }
}
