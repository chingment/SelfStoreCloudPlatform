using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.StaffUser
{
    public class EditViewModel : OwnBaseViewModel
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
        public EditViewModel()
        {

        }

        public EditViewModel(int id)
        {
            var sysStaffUser = CurrentDb.SysStaffUser.Where(m => m.Id == id).FirstOrDefault();
            if (sysStaffUser != null)
            {
                _sysStaffUser = sysStaffUser;
            }
        }
    }
}