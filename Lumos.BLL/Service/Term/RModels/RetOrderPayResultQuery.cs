using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class RetOrderPayResultQuery
    {
        public string OrderSn { get; set; }

        public Entity.Enumeration.OrderStatus Status { get; set; }
    }
}
