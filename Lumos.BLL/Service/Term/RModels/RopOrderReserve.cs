using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{


    public class RopOrderReserve
    {
        public RopOrderReserve()
        {
            this.Details = new List<Detail>();
        }
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public Enumeration.OrderSource Source { get; set; }
        public Enumeration.ReserveMode ReserveMode { get; set; }
        public string ChannelId { get; set; }
        public Enumeration.ChannelType ChannelType { get; set; }
        public string PayWay { get; set; }
        public string Receiver { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceptionAddress { get; set; }
        public int PayTimeout { get; set; }
        public List<Detail> Details { get; set; }
        public class Detail
        {
            public string SkuId { get; set; }
            public int Quantity { get; set; }
            public Enumeration.ReceptionMode ReceptionMode { get; set; }
        }
    }
}
