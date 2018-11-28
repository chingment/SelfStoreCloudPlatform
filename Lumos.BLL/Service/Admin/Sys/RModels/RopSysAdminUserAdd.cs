using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.Entity;
namespace Lumos.BLL.Service.Admin
{
    public class RopSysAdminUserAdd
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Enumeration.SysPositionId PositionId { get; set; }

        public string OrganizationId { get; set; }
    }
}
