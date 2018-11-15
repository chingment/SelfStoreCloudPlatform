using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class RopSysStaffUserEdit
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string[] RoleIds { get; set; }
    }
}
