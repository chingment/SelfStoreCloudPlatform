using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class RopSysMenuEdit
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string MenuId { get; set; }

        public string[] PermissionIds { get; set; }
    }
}
