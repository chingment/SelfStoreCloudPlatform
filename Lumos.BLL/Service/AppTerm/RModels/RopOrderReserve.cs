using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{


    public class RopOrderReserve
    {
        public RopOrderReserve()
        {
            this.Skus = new List<Sku>();
        }

        public string MachineId { get; set; }
        public int PayTimeout { get; set; }
        public List<Sku> Skus { get; set; }

        public class Sku
        {
            public string Id { get; set; }
            public int Quantity { get; set; }
            //public Enumeration.ReceptionMode ReceptionMode { get; set; }
        }
    }
}
