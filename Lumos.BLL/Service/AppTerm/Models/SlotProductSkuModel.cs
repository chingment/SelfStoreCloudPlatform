using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public class SlotProductSkuModel
    {
        public string Id { get; set; }
        public string SlotId { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public int Quantity { get; set; }
        public int LockQuantity { get; set; }
        public int SellQuantity { get; set; }
    }
}
