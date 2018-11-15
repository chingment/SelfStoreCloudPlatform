using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public static class TermServiceFactory
    {
        public static OrderService Order
        {
            get
            {
                return new OrderService();
            }
        }

        public static MachineService Machine
        {
            get
            {
                return new MachineService();
            }
        }

        public static ProductKindService ProductKind
        {
            get
            {
                return new ProductKindService();
            }
        }

        public static GlobalService Global
        {
            get
            {
                return new GlobalService();
            }
        }
    }
}
