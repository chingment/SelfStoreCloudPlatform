using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Merchant
{
    public class AddViewModel : OwnBaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();
        private Lumos.Entity.SysMerchatUser _sysMerchatUser = new Lumos.Entity.SysMerchatUser();

        public Lumos.Entity.Merchant Merchant
        {
            get
            {
                return _merchant;
            }
            set
            {
                _merchant = value;
            }
        }

        public Lumos.Entity.SysMerchatUser SysMerchatUser
        {
            get
            {
                return _sysMerchatUser;
            }
            set
            {
                _sysMerchatUser = value;
            }
        }
    }
}