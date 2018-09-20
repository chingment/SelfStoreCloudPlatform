using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class ProductSkuDetailsModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }
        public string UnitPrice { get; set; }

        public string ShowPrice { get; set; }

        public string BriefIntro { get; set; }

        public List<Lumos.Entity.ImgSet> DispalyImgs { get; set; }

        public List<SpecModel> Specs { get; set; }

        public string Details { get; set; }

        public string ServiceDesc { get; set; }
    }
}
