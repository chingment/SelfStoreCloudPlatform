using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class ProductParentKindModel
    {
        public ProductParentKindModel()
        {
            this.Child = new List<ProductChildKindModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public bool Selected { get; set; }

        public List<ProductChildKindModel> Child { get; set; }
    }
}
