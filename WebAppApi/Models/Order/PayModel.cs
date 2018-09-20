using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class PayModel
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public string OrderSn { get; set; }

    }
}