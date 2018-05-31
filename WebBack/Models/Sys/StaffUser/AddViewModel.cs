using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.StaffUser
{
    public class AddViewModel:OwnViewModel
    {
        private SysStaffUser _sysStaffUser = new SysStaffUser();

        public SysStaffUser SysStaffUser
        {
            get
            {
                return _sysStaffUser;
            }
            set
            {
                _sysStaffUser = value;
            }
        }

        public int[] UserRoleIds { get; set; }
    }
}