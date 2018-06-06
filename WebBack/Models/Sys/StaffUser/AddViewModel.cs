using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.StaffUser
{
    public class AddViewModel:OwnBaseViewModel
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

        public string[] UserRoleIds { get; set; }
    }
}