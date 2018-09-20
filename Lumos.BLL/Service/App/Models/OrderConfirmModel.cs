using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class OrderConfirmModel
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public List<OrderConfirmSkuModel> Skus { get; set; }
        public List<int> CouponId { get; set; }

    }
}
