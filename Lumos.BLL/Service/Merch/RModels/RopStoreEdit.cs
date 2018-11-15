using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RopStoreEdit
    {
        public string StoreId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public Lumos.Entity.Enumeration.StoreStatus Status { get; set; }
    }
}
