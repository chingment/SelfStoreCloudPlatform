﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RetProductKindGetDetails
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string IconImg { get; set; }

        public string MainImg { get; set; }

        public string Description { get; set; }

        public Lumos.Entity.Enumeration.ProductKindStatus Status { get; set; }
    }
}
