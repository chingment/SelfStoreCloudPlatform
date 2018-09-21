
using Lumos.BLL.Service.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Global
{
    public class DataSetResultModel
    {
        public IndexModel Index{ get; set; }

        public ProductKindModel ProductKind { get; set; }

        public CartModel Cart { get; set; }

        public PersonalModel Personal { get; set; }
    }
}