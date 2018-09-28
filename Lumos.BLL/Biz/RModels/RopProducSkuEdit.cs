using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz.RModels
{
    public class RopProducSkuEdit
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string BarCode { get; set; }

        public string SimpleCode { get; set; }

        public List<ImgSet> DispalyImgUrls { get; set; }

        public decimal ShowPrice { get; set; }

        public decimal SalePrice { get; set; }

        public string DetailsDes { get; set; }

        public string SpecDes { get; set; }

        public string BriefInfo { get; set; }

        public List<string> KindIds { get; set; }

        public List<string> RecipientModeIds { get; set; }

        public List<string> SubjectIds
        {
            get; set;
        }
    }
}
