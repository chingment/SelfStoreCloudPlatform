using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.Menu
{
    public class DetailsViewModel : OwnBaseViewModel
    {
        private SysMenu _sysMenu = new SysMenu();
    
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

        public DetailsViewModel()
        {

        }
        public DetailsViewModel(string id)
        {
            var sysMenu = CurrentDb.SysMenu.Where(m => m.Id == id).FirstOrDefault();
            if (sysMenu != null)
            {
                _sysMenu = sysMenu;

                var sysMenuPermission = CurrentDb.SysMenuPermission.Where(u => u.MenuId == id).ToList();
                if(sysMenuPermission!=null)
                {
                    _sysMenuPermission = sysMenuPermission;
                }
            }


        }



    }
}