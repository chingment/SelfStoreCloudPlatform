using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class RetOrderConfirm
    {
        public RetOrderConfirm()
        {
            this.SubtotalItem = new List<OrderConfirmSubtotalItemModel>();
            this.Block = new List<OrderBlockModel>();
        }

        //选择的优惠卷
        public OrderConfirmCouponModel Coupon { get; set; }
        //订单块
        public List<OrderBlockModel> Block { get; set; }
        //小计项目
        public List<OrderConfirmSubtotalItemModel> SubtotalItem { get; set; }
        //实际支付金额
        public string ActualAmount { get; set; }
        //原金额
        public string OriginalAmount { get; set; }

        public string OrderId { get; set; }

    }
}
