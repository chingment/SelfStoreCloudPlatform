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

    public class OrderConfirmSubtotalItemModel
    {
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
        public bool IsDcrease { get; set; }
    }
}
