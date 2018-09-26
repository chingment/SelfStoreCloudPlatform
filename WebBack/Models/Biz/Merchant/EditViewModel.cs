using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Merchant
{
    public class EditViewModel : OwnBaseViewModel
    {
        private Lumos.Entity.MerchantConfig _merchant = new Lumos.Entity.MerchantConfig();
        private Lumos.Entity.SysMerchantUser _sysMerchantUser = new Lumos.Entity.SysMerchantUser();

        public Lumos.Entity.MerchantConfig MerchantConfig
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

        public Lumos.Entity.SysMerchantUser SysMerchantUser
        {
            get
            {
                return _sysMerchantUser;
            }
            set
            {
                _sysMerchantUser = value;
            }
        }

        public EditViewModel()
        {

        }

        public EditViewModel(string id)
        {
            var merchant = CurrentDb.MerchantConfig.Where(m => m.Id == id).FirstOrDefault();
            if (merchant != null)
            {
                _merchant = merchant;

                var sysMerchatUser = CurrentDb.SysMerchantUser.Where(m => m.Id == merchant.MerchantId).FirstOrDefault();

                if (sysMerchatUser != null)
                {
                    _sysMerchantUser = sysMerchatUser;
                }

            }
        }
    }
}