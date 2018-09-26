using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("ProductSku")]
    public class ProductSku
    {
        [Key]
        public string Id { get; set; }
        public string MerchantId { get; set; }
        public string KindIds { get; set; }
        public string KindNames { get; set; }
        public string RecipientModeIds { get; set; }
        public string RecipientModeNames { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }
        public string SimpleCode { get; set; }
        public string ImgUrl { get; set; }
        public string DispalyImgUrls { get; set; }
        public decimal ShowPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string DetailsDes { get; set; }
        public string SpecDes { get; set; }
        public string BriefInfo { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? MendTime { get; set; }
        public string Mender { get; set; }
    }


}
