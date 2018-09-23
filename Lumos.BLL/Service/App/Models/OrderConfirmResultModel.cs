using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{

    public class OrderBlock
    {
        public OrderBlock()
        {
            this.Skus = new List<RopOrderConfirm.SkuModel>();
        }

        public string TagName { get; set; }
        public UserDeliveryAddressModel ShippingAddress { get; set; }

        public List<RopOrderConfirm.SkuModel> Skus { get; set; }
    }

    public class OrderConfirmResultModel
    {
        public OrderConfirmResultModel()
        {
            this.SubtotalItem = new List<OrderConfirmSubtotalItemModel>();
        }

        //选择的优惠卷
        public OrderConfirmCouponModel Coupon { get; set; }
        //订单块
        public List<OrderBlock> Block { get; set; }
        //小计项目
        public List<OrderConfirmSubtotalItemModel> SubtotalItem { get; set; }
        //实际支付金额
        public string ActualAmount { get; set; }
        //原金额
        public string OriginalAmount { get; set; }
    }
}
