
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public class RetOrderPayStatusQuery
    {
        public string OrderId { get; set; }

        public string OrderSn { get; set; }

        public Entity.Enumeration.OrderStatus Status { get; set; }

        public RetOrderDetails OrderDetails { get; set; }

    }
}
