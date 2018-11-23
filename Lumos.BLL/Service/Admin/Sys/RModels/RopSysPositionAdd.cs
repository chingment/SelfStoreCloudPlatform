using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class RopSysPositionAdd
    {
        public string Name { get; set; }

        public string OrganizationId { get; set; }

        public string[] RoleIds { get; set; }
    }
}
