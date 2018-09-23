﻿using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class CartSkuModel : SkuModel
    {
        public string CartId { get; set; }
        public int Quantity { get; set; }
        public bool Selected { get; set; }
        public decimal SumPrice { get; set; }
        public int ChannelId { get; set; }
        public Enumeration.ChannelType ChannelType { get; set; }
    }
}
