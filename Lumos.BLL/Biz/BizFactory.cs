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

        public static ProductSubjectProvider ProductSubject
        {
            get
            {
                return new ProductSubjectProvider();
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

        public static CompanyProvider Company
        {
            get
            {
                return new CompanyProvider();
            }
        }

        public static Order2StockInProvider Order2StockIn
        {
            get
            {
                return new Order2StockInProvider();
            }
        }

        public static OrderProvider Order
        {
            get
            {
                return new OrderProvider();
            }
        }

        public static WxUserProvider WxUser
        {
            get
            {
                return new WxUserProvider();
            }
        }

        public static RecipientModeProvider RecipientMode
        {
            get
            {
                return new RecipientModeProvider();
            }
        }

        public static SmsProvider Sms
        {
            get
            {
                return new SmsProvider();
            }
        }

    }
}
