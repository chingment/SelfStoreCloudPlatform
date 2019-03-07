using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class SkuModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public decimal SalePrice { get; set; }
        public decimal ShowPrice { get; set; }
        public string BriefInfo { get; set; }
        public List<Lumos.Entity.ImgSet> DispalyImgUrls { get; set; }
        public string DetailsDes { get; set; }
        public string SpecDes { get; set; }
    }
}
