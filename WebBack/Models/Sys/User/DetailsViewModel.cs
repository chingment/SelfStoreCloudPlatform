using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.User
{
    public class DetailsViewModel : OwnViewModel
    {

        private SysUser _sysUser = new SysUser();

        public SysUser SysUser
        {
            get
            {
                return _sysUser;
            }
            set
            {
                _sysUser = value;
            }
        }

        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
        {
            var sysUser = CurrentDb.SysUser.Where(m => m.Id == id).FirstOrDefault();
            if (sysUser != null)
            {
                _sysUser = sysUser;
            }
        }

    }
}