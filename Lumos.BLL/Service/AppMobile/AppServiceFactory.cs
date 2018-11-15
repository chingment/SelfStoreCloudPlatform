using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
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

        public static ProductSkuService ProductSku
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

        public static UserDeliveryAddressService UserDeliveryAddress
        {
            get
            {
                return new UserDeliveryAddressService();
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

        public static StoreService Store
        {
            get
            {
                return new StoreService();
            }
        }

        public static OperateService Operate
        {
            get
            {
                return new OperateService();
            }
        }

        public static MachineService Machine
        {
            get
            {
                return new MachineService();
            }
        }

        public static ReplenishStaffService ReplenishStaff
        {
            get
            {
                return new ReplenishStaffService();
            }
        }

    }
}
