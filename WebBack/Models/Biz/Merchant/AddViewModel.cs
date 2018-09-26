using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Merchant
{
    public class AddViewModel : OwnBaseViewModel
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
    }
}