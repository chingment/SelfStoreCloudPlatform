﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class RetStoreGetDetails
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public string StoreId { get; set; }

        public Lumos.Entity.Enumeration.StoreStatus Status { get; set; }
    }
}