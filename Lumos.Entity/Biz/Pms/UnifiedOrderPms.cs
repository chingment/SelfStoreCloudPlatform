using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public enum UnifiedOrderType
    {

        [Remark("未知")]
        Unknow = 0,
        [Remark("购买促销优惠卷")]
        BuyPromoteCoupon = 1
    }

    public class UnifiedOrderPms
    {
        public UnifiedOrderType Type { get; set; }
        public string RefereeId { get; set; }
        public Dictionary<string, string> OrderPms { get; set; }
    }
}
