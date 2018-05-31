using Lumos.BLL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.Menu
{
    public class ListViewModel:OwnBaseViewModel
    {
        private List<SysPermission> _sysPermission = new List<SysPermission>();

        public List<SysPermission> SysPermission
        {
            get
            {
                return _sysPermission;
            }
            set
            {
                _sysPermission = value;
            }
        }

        public ListViewModel()
        {
            var sysPermission = SysFactory.AuthorizeRelay.GetPermissionList(new PermissionCode());

            _sysPermission = sysPermission;

        }
    }
}