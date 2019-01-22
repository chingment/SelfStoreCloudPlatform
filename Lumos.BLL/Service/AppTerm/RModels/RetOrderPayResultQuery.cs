using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public class RetOrderPayResultQuery
    {
        public string OrderSn { get; set; }

        public Entity.Enumeration.OrderStatus Status { get; set; }

        public List<SkuModel> Skus { get; set; }

        public class SkuModel
        {

            public string Id { get; set; }

            public string Name { get; set; }

            public string SlotId { get; set; }

            public int Quantity { get; set; }

            public string Status { get; set; }

            public string StatusName { get; set; }
        }
    }
}
