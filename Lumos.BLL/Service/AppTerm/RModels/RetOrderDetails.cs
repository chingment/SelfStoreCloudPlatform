using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public class RetOrderDetails
    {
        public RetOrderDetails()
        {
            this.Skus = new List<Sku>();
        }

        public List<Sku> Skus { get; set; }

        public class Sku
        {
            public Sku()
            {
                this.Slots = new List<Slot>();
            }

            public string Id { get; set; }
            public string ImgUrl { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public int QuantityBySuccess { get; set; }

            public List<Slot> Slots { get; set; }
        }

        public class Slot
        {
            public string Id { get; set; }

            public Enumeration.OrderDetailsChildSonStatus Status { get; set; }
        }
    }
}
