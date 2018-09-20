using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class OrderConfirmSubtotalItemModel
    {
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
        public bool IsDcrease { get; set; }
    }
}
