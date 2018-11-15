using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class RopOrderConfirm
    {
        public string OrderId { get; set; }
        public string StoreId { get; set; }
        public List<OrderConfirmSkuModel> Skus { get; set; }
        public List<string> CouponId { get; set; }

    }
}
