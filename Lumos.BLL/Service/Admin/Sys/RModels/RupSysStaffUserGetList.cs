using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class RupSysStaffUserGetList : RupBaseGetList
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
