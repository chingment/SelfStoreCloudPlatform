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
        public CustomJsonResult Add(string pOperater, Company pCompany)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var existObject = CurrentDb.Company.Where(m => m.UserId == pCompany.UserId && m.Name == pCompany.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }
                pCompany.Id = GuidUtil.New();
                //warehouse.Status = Enumeration.StoreStatus.Closed;
                pCompany.CreateTime = this.DateTime;
                pCompany.Creator = pOperater;
                CurrentDb.Company.Add(pCompany);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

            }
            return result;
        }

        public CustomJsonResult Edit(string pOperater, Company pCompany)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var lCompany = CurrentDb.Company.Where(m => m.Id == pCompany.Id).FirstOrDefault();

                var existObject = CurrentDb.Company.Where(m => m.UserId == lCompany.UserId && m.Id != pCompany.Id && m.Name == pCompany.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }



                lCompany.Name = pCompany.Name;
                lCompany.Address = pCompany.Address;
                lCompany.Description = pCompany.Description;
                // l_Warehouse.Status = store.Status;
                lCompany.MendTime = this.DateTime;
                lCompany.Mender = pOperater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }
    }
}
