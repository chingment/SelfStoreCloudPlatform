using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class RopOrderConfirm
    {
        public string OrderId { get; set; }
        public List<SkuModel> Skus { get; set; }
        public List<string> CouponId { get; set; }

        public class SkuModel
        {
            public string CartId { get; set; }

            public string SkuId { get; set; }

            public int Quantity { get; set; }

            public string SkuName { get; set; }

            public string SkuImgUrl { get; set; }

            public string SalesPrice { get; set; }

            public string SalesPriceByVip { get; set; }

            public int ChannelId { get; set; }
            public Enumeration.ChannelType ChannelType { get; set; }

        }

    }
}
