using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.Banner
{
    public class AddBannerViewModel : OwnBaseViewModel
    {
        private Lumos.Entity.SysBanner _sysBanner = new Lumos.Entity.SysBanner();

        public Lumos.Entity.SysBanner SysBanner
        {
            get
            {
                return _sysBanner;
            }
            set
            {
                _sysBanner = value;
            }
        }

        public AddBannerViewModel()
        {

        }
    }
}