using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class BizFactory
    {
        public static ProductSkuProvider ProductSku
        {
            get
            {
                return new ProductSkuProvider();
            }
        }
    }
}
