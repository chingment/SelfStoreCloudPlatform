using Lumos.BLL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.Menu
{
    public class AddViewModel : OwnBaseViewModel
    {
        private SysMenu _sysMenu = new SysMenu();
        private List<SysPermission> _sysPermission = new List<SysPermission>();

        public SysMenu SysMenu
        {
            get
            {
                return _sysMenu;
            }
            set
            {
                _sysMenu = value;
            }
        }

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

        public AddViewModel()
        {


            var sysPermission = SysFactory.AuthorizeRelay.GetPermissionList(new PermissionCode());

            _sysPermission = sysPermission;
        }
    }
}