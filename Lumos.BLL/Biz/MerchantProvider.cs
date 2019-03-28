using Lumos.WeiXinSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class MerchantProvider : BaseProvider
    {
        public WxAppInfoConfig GetWxAppInfoConfig(string id)
        {

            var config = new WxAppInfoConfig();

            var merchant = CurrentDb.Merchant.Where(m => m.Id == id).FirstOrDefault();
            if (merchant == null)
                return null;

            return config;
        }
    }
}
