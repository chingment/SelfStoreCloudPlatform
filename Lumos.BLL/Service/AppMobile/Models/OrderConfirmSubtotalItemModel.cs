using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class OrderBlockModel
    {
        public OrderBlockModel()
        {
            this.Skus = new List<OrderConfirmSkuModel>();
        }

        public string TagName { get; set; }
        public UserDeliveryAddressModel DeliveryAddress { get; set; }

        public List<OrderConfirmSkuModel> Skus { get; set; }
    }

    public class OrderConfirmSubtotalItemModel
    {
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
        public bool IsDcrease { get; set; }
    }
}
