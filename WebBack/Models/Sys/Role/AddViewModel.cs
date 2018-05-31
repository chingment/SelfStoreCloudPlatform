using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.Role
{
    public class AddViewModel: OwnViewModel
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
    }
}