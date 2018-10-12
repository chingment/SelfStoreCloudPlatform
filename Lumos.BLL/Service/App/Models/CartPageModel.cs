using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{

    public class CartBlock
    {

        public string TagName { get; set; }

        public List<CartSkuModel> Skus { get; set; }

        public Entity.Enumeration.ReceptionMode ReceptionMode { get; set; }



    }

    public class CartPageModel
    {
        public CartPageModel()
        {
            this.Block = new List<CartBlock>();
        }

        public List<CartBlock> Block { get; set; }

        public int Count { get; set; }

        public decimal SumPrice { get; set; }

        public int CountBySelected { get; set; }

        public decimal SumPriceBySelected { get; set; }
    }
}
