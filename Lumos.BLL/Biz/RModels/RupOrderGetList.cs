using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class RupOrderGetList : RupBaseGetList
    {
        public string Nickname { get; set; }

        public Enumeration.OrderStatus OrderStatus { get; set; }

        public string OrderSn { get; set; }
    }
}
