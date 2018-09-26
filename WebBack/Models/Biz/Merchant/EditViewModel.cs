using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Merchant
{
    public class EditViewModel : OwnBaseViewModel
    {
        private Lumos.Entity.SysMerchantUser _sysMerchantUser = new Lumos.Entity.SysMerchantUser();

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


            var sysMerchatUser = CurrentDb.SysMerchantUser.Where(m => m.Id == id).FirstOrDefault();

            if (sysMerchatUser != null)
            {
                _sysMerchantUser = sysMerchatUser;
            }


        }
    }
}