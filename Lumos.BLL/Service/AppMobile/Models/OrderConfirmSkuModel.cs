using Lumos.BLL.Biz;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class OrderConfirmSkuModel:SkuModel
    {
        public string CartId { get; set; }
        public int Quantity { get; set; }
        public decimal SalePriceByVip { get; set; }
        public Enumeration.ReceptionMode ReceptionMode { get; set; }
    }
}
