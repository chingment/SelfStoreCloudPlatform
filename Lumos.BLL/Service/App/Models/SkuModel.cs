using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class SkuModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string SalePrice { get; set; }
        public string ShowPrice { get; set; }
        public string BriefIntro { get; set; }
        public List<Lumos.Entity.ImgSet> DispalyImgUrls { get; set; }
        public string DetailsDes { get; set; }
    }
}
