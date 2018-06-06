using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebBack.Models.Sys.Role
{
    public class EditViewModel:OwnBaseViewModel
    {
        private SysRole _sysRole = new SysRole();

        public SysRole SysRole
        {
            get
            {
                return _sysRole;
            }
            set
            {
                _sysRole = value;
            }
        }

        public EditViewModel()
        {

        }


        public EditViewModel(string id)
        {
            var sysRole = CurrentDb.SysRole.Where(m => m.Id == id).FirstOrDefault();
            if(sysRole!=null)
            {
                _sysRole = sysRole;
            }
        }
    }
}