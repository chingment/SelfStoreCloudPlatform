using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz.RModels
{
    public class RopOrderReserve
    {
        public RopOrderReserve()
        {
            this.Skus = new List<Sku>();
        }
        public string StoreId { get; set; }
        public string ClientId { get; set; }
        public Enumeration.OrderSource Source { get; set; }
        public Enumeration.ReserveMode ReserveMode { get; set; }
        public string ChannelId { get; set; }
        public Enumeration.ChannelType ChannelType { get; set; }
        public string PayWay { get; set; }
        public string Receiver { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceptionAddress { get; set; }
        public int PayTimeout { get; set; }
        public List<Sku> Skus { get; set; }
        public class Sku
        {
            public string Id { get; set; }
            public int Quantity { get; set; }
            public Enumeration.ReceptionMode ReceptionMode { get; set; }
        }
    }
}
