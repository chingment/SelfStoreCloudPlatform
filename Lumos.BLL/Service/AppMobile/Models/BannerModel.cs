using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class BannerModel
    {
        public bool Autoplay { get; set; }
        public int CurrentSwiper { get; set; }
        public BannerModel()
        {
            this.Imgs = new List<ImgModel>();
        }

        public List<ImgModel> Imgs { get; set; }

        public class ImgModel
        {
            public string Id { get; set; }

            public string Title { get; set; }

            public string Link { get; set; }

            public string Url { get; set; }
        }
    }
}
