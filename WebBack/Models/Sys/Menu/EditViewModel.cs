using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.Menu
{
    public class EditViewModel: OwnBaseViewModel
    {
        private SysMenu _sysMenu = new SysMenu();
        private List<SysPermission> _sysPermission = new List<SysPermission>();
        private List<SysMenuPermission> _sysMenuPermission = new List<SysMenuPermission>();

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
        public List<SysMenuPermission> SysMenuPermission
        {
            get
            {
                return _sysMenuPermission;
            }
            set
            {
                _sysMenuPermission = value;
            }
        }

        public EditViewModel()
        {

        }
    }
}