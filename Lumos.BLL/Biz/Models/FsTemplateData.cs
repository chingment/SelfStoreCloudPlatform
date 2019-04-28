using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class FsTemplateData
    {
        public string Type { get; set; }

        public object Value { get; set; }


        public class TmplOrderSku
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string ImgUrl { get; set; }
            public string Quantity { get; set; }
            public string ChargeAmount { get; set; }
            public bool IsGift { get; set; }
            public FsText Desc { get; set; }
        }
    }
}
