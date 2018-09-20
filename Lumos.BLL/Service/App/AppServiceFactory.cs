﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class AppServiceFactory : BaseFactory
    {
        public static IndexService Index
        {
            get
            {
                return new IndexService();
            }
        }

        public static ProductSkuService Product
        {
            get
            {
                return new ProductSkuService();
            }
        }

        public static CartService Cart
        {
            get
            {
                return new CartService();
            }
        }

        public static PersonalService Personal
        {
            get
            {
                return new PersonalService();
            }
        }

        public static ShippingAddressService ShippingAddress
        {
            get
            {
                return new ShippingAddressService();
            }
        }

        public static OrderService Order
        {
            get
            {
                return new OrderService();
            }
        }

        public static CouponService Coupon
        {
            get
            {
                return new CouponService();
            }
        }
        public static ProductKindService ProductKind
        {
            get
            {
                return new ProductKindService();
            }
        }

    }
}
