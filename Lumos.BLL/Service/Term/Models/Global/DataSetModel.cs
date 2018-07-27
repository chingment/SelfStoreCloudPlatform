using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term.Models.Global
{
    public class DataSetModel
    {
        public DataSetModel()
        {
            this.Banners = new List<BannerModel>();
            this.ProductSkus = new Dictionary<string, ProductSkuModel>();
            this.ProductKinds = new List<ProductParentKindModel>();
        }
        public string BtnBuyImgUrl { get; set; }
        public string BtnPickImgUrl { get; set; }
        public string LogoImgUrl { get; set; }
        public List<BannerModel> Banners { get; set; }

        public Dictionary<string, ProductSkuModel> ProductSkus { get; set; }

        public List<ProductParentKindModel> ProductKinds { get; set; }

    }
}
