using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class RupCouponMy
    {
        public bool IsGetHis { get; set; }
        public List<OrderConfirmSkuModel> Skus { get; set; }
        public List<string> CouponId { get; set; }
    }
}
