using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiTerm
{
    public class RetOrderSkuPickupStatusQuery
    {
        public Entity.Enumeration.OrderDetailsChildSonStatus Status { get; set; }

        public string Tips { get; set; }
    }
}
