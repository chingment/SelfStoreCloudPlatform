using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Merch
{
    public class CompanyProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string merchantId, string id)
        {
            var ret = new RetCompanyGetDetails();
            var warehouse = CurrentDb.Company.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
            if (warehouse != null)
            {
                ret.Id = warehouse.Id ?? "";
                ret.Name = warehouse.Name ?? "";
                ret.Address = warehouse.Address ?? "";
                ret.Description = warehouse.Description ?? "";
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }


        public CustomJsonResult Add(string operater, string merchantId, RopCompanyAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var existObject = CurrentDb.Company.Where(m => m.MerchantId == merchantId && m.Name == rop.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }

                var company = new Company();
                company.Id = GuidUtil.New();
                company.MerchantId = merchantId;
                company.Name = rop.Name;
                company.Address = rop.Address;
                company.Class = rop.Class;
                company.Description = rop.Description;
                company.CreateTime = this.DateTime;
                company.Creator = operater;
                CurrentDb.Company.Add(company);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

            }
            return result;
        }

        public CustomJsonResult Edit(string operater, string merchantId, RopCompanyEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var company = CurrentDb.Company.Where(m => m.Id == rop.Id).FirstOrDefault();

                var existObject = CurrentDb.Company.Where(m => m.MerchantId == merchantId && m.Id != rop.Id && m.Name == rop.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }

                company.Name = rop.Name;
                company.Address = rop.Address;
                company.Description = rop.Description;
                company.MendTime = this.DateTime;
                company.Mender = operater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }
    }
}
