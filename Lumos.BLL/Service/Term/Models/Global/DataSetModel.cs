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
            this.ProductSkus = new List<ProductSkuModel>();
            this.ProductKinds = new List<ProductKindModel>();
        }
        public string BtnBuyImgUrl { get; set; }
        public string BtnPickImgUrl { get; set; }
        public string LogoImgUrl { get; set; }
        public List<BannerModel> Banners { get; set; }

        public List<ProductSkuModel> ProductSkus { get; set; }

        public List<ProductKindModel> ProductKinds { get; set; }

    }
}
