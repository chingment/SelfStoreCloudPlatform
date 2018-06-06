using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Merchant
{
    public class EditViewModel : OwnBaseViewModel
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

        public EditViewModel()
        {

        }

        public EditViewModel(string id)
        {
            var merchant = CurrentDb.Merchant.Where(m => m.Id == id).FirstOrDefault();
            if (merchant != null)
            {
                _merchant = merchant;

                var sysMerchatUser = CurrentDb.SysMerchatUser.Where(m => m.Id == merchant.UserId).FirstOrDefault();

                if (sysMerchatUser != null)
                {
                    _sysMerchatUser = sysMerchatUser;
                }

            }
        }
    }
}