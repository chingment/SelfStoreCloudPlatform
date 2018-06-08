using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term.Models
{
    public class ProductKindModel
    {
        public ProductKindModel()
        {
            this.Banners = new List<BannerModel>();
            this.Childs = new List<ProductChildKindModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public List<BannerModel> Banners { get; set; }

        public List<ProductChildKindModel> Childs { get; set; }
    }
}
