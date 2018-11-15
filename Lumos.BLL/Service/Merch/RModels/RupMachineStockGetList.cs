using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RupMachineStockGetList:RupBaseGetList
    {
        public string StoreId { get; set; }

        public string MachineId { get; set; }
    }
}
