using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class SkuModel
    {
        public string SkuId { get; set; }
        public string SkuName { get; set; }
        public string SkuImgUrl { get; set; }
        public string SalePrice { get; set; }
        public string ShowPrice { get; set; }
        public string BriefIntro { get; set; }
        public List<Lumos.Entity.ImgSet> DispalyImgUrls { get; set; }
        public string DetailsDes { get; set; }
    }
}
