﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class PdAreaModel
    {
        public PdAreaModel()
        {

            this.Tabs = new List<Tab>();
        }
        public int TabsSliderIndex { get; set; }

        public List<Tab> Tabs { get; set; }

        public class Tab
        {
            public string BannerImgUrl { get; set; }
            public string Name { get; set; }
            public bool Selected { get; set; }
            public List<SkuModel> List { get; set; }
            public int PageIndex { get; set; }
        }
    }
}
