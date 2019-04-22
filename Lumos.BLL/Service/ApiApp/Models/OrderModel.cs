using Lumos.BLL.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class OrderModel
    {
        public OrderModel()
        {
            this.Blocks = new List<BlockModel>();
        }
        public int Id { get; set; }

        public string Sn { get; set; }

        public string StatusName { get; set; }

        public string Remarks { get; set; }

        public List<BlockModel> Blocks { get; set; }

        public string ChargeAmount { get; set; }

        public class BlockModel
        {
            public BlockModel()
            {
                this.Skus = new List<MySkuModel>();
            }

            public string TagName { get; set; }

            public List<MySkuModel> Skus { get; set; }
        }

        public class MySkuModel : SkuModel
        {
            public int Quantity { get; set; }
            public decimal SumPrice { get; set; }
        }
    }
}
