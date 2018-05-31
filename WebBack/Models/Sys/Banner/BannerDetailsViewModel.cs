using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.Banner
{
    public class BannerDetailsViewModel : OwnBaseViewModel
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

        public BannerDetailsViewModel()
        {

        }
        public BannerDetailsViewModel(int id)
        {

            var sysBanner = CurrentDb.SysBanner.Where(m => m.Id == id).FirstOrDefault();
            if (sysBanner != null)
            {
                _sysBanner = sysBanner;
            }
        }
    }
}