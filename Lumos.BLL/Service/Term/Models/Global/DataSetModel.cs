using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term.Models.Global
{
    public class DataSetModel
    {
        public string BtnBuyImgUrl { get; set; }
        public string BtnPickImgUrl { get; set; }
        public string LogoImgUrl { get; set; }
        public List<BannerModel> Banner { get; set; }
    }
}
