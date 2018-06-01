using Lumos.BLL.Service.Term;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class TermServiceFactory
    {
        public static MachineService Machine
        {
            get
            {
                return new MachineService();
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
