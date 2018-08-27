using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class CompanyProvider : BaseProvider
    {
        public CustomJsonResult Add(string operater, Company company)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var existObject = CurrentDb.Company.Where(m => m.UserId == company.UserId && m.Name == company.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }
                company.Id = GuidUtil.New();
                //warehouse.Status = Enumeration.StoreStatus.Closed;
                company.CreateTime = this.DateTime;
                company.Creator = operater;
                CurrentDb.Company.Add(company);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

            }
            return result;
        }

        public CustomJsonResult Edit(string operater, Company company)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var l_Company = CurrentDb.Company.Where(m => m.Id == company.Id).FirstOrDefault();

                var existObject = CurrentDb.Company.Where(m => m.UserId == l_Company.UserId && m.Id != company.Id && m.Name == company.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }



                l_Company.Name = company.Name;
                l_Company.Address = company.Address;
                l_Company.Description = company.Description;
                // l_Warehouse.Status = store.Status;
                l_Company.MendTime = this.DateTime;
                l_Company.Mender = operater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }
    }
}
