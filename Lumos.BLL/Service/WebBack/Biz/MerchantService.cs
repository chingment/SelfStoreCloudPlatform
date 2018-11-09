using Lumos.DAL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.WebBack
{
    public class MerchantService : BaseProvider
    {
        public CustomJsonResult GetDetails(string pOperater, string manchineId)
        {
            var ret = new RetMerchantGetDetails();
            var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == manchineId).FirstOrDefault();
            if (sysMerchantUser != null)
            {
                ret.MerchantId = sysMerchantUser.Id ?? ""; ;
                ret.UserName = sysMerchantUser.UserName ?? ""; ;
                ret.MerchantName = sysMerchantUser.MerchantName ?? ""; ;
                ret.ContactAddress = sysMerchantUser.ContactAddress ?? "";
                ret.ContactName = sysMerchantUser.ContactName ?? "";
                ret.ContactPhone = sysMerchantUser.ContactPhone ?? "";
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string pOperater, RopMerchantAdd rop)
        {
            var sysMerchantUser = new SysMerchantUser();
            sysMerchantUser.UserName = string.Format("{0}{1}", "M", rop.UserName);
            sysMerchantUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
            sysMerchantUser.MerchantName = rop.MerchantName;
            sysMerchantUser.ContactName = rop.ContactName;
            sysMerchantUser.ContactAddress = rop.ContactAddress;
            sysMerchantUser.ContactPhone = rop.ContactPhone;
            return BizFactory.Merchant.Add(pOperater, sysMerchantUser);
        }

        public CustomJsonResult Edit(string pOperater, RopMerchantEdit rop)
        {
            var sysMerchantUser = new SysMerchantUser();
            sysMerchantUser.Id = rop.MerchantId;

            if (string.IsNullOrEmpty(rop.Password))
            {
                sysMerchantUser.PasswordHash = PassWordHelper.HashPassword(rop.Password);
            }

            sysMerchantUser.PasswordHash = rop.Password;
            sysMerchantUser.MerchantName = rop.MerchantName;
            sysMerchantUser.ContactName = rop.ContactName;
            sysMerchantUser.ContactAddress = rop.ContactAddress;
            sysMerchantUser.ContactPhone = rop.ContactPhone;

            return BizFactory.Merchant.Edit(pOperater, sysMerchantUser);
        }
    }
}
