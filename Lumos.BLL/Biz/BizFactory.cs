using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class BizFactory : BaseFactory
    {
        public static MachineProvider Machine
        {
            get
            {
                return new MachineProvider();
            }
        }

        public static MerchantProvider Merchant
        {
            get
            {
                return new MerchantProvider();
            }
        }

        public static MerchantMachineProvider MerchantMachine
        {
            get
            {
                return new MerchantMachineProvider();
            }
        }

        public static ProductKindProvider ProductKind
        {
            get
            {
                return new ProductKindProvider();
            }
        }

        public static ProductSkuProvider ProductSku
        {
            get
            {
                return new ProductSkuProvider();
            }
        }

        public static StoreProvider Store
        {
            get
            {
                return new StoreProvider();
            }
        }

        public static WarehouseProvider Warehouse
        {
            get
            {
                return new WarehouseProvider();
            }
        }

    }
}
