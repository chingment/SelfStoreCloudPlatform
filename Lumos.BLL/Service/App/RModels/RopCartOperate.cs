﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class RopCartOperate
    {
        public string StoreId { get; set; }
        public Lumos.Entity.Enumeration.CartOperateType Operate { get; set; }
        public List<SkuModel> Skus { get; set; }


        public class SkuModel
        {
            public string SkuId { get; set; }
            public int Quantity { get; set; }
            public bool Selected { get; set; }
            public int ChannelId { get; set; }
            public Entity.Enumeration.ChannelType ChannelType { get; set; }
        }
    }
}