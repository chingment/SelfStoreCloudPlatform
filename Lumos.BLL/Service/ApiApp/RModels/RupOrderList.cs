using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class RupOrderList
    {
        public string StoreId { get; set; }
        public int PageIndex { get; set; }

        public Enumeration.OrderStatus Status { get; set; }

        public AppCaller Caller { get; set; }
    }
}
