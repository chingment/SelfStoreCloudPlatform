using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public class ProductSkuModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string SalePrice { get; set; }
        public string ShowPirce { get; set; }
        [JsonConverter(typeof(JsonObjectConvert))]
        public string DisplayImgUrls { get; set; }
        public string SpecDes { get; set; }
        public string BriefInfo { get; set; }
        public string DetailsDes { get; set; }
        public int Quantity { get; set; }
        public int LockQuantity { get; set; }
        public int SellQuantity { get; set; }

    }
}
