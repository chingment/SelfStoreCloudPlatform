﻿using Lumos.BLL.Service.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Coupon
{
    public class GetListParams
    {
        public string UserId { get; set; }
        public bool IsGetHis { get; set; }
        public List<OrderConfirmSkuModel> Skus { get; set; }

        public List<string> CouponId { get; set; }
    } 
}