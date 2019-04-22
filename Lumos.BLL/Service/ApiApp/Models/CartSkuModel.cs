using Lumos.BLL.Biz;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class CartSkuModel : SkuModel
    {
        public string CartId { get; set; }
        public int Quantity { get; set; }
        public bool Selected { get; set; }
        public decimal SumPrice { get; set; }
        public Enumeration.ReceptionMode ReceptionMode { get; set; }
    }
}
