using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{


    public class RupSysUserGetList : RupBaseGetList
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
