using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.Entity;

namespace Lumos.BLL.Service.Admin
{
   public  class RopSysPositionEdit
    {
        public Enumeration.SysPositionId Id { get; set; }
        public string[] RoleIds { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
