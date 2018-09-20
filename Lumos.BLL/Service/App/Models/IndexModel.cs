using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class IndexModel
    {
        public IndexModel()
        {

            this.Banner = new List<BannerModel>();
        }

        public List<BannerModel> Banner { get; set; }
    }
}
