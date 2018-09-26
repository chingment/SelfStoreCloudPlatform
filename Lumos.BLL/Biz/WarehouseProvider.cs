using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class WarehouseProvider : BaseProvider
    {
        public CustomJsonResult Add(string pOperater, Warehouse pWarehouse)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var existObject = CurrentDb.Warehouse.Where(m => m.MerchantId == pWarehouse.MerchantId && m.Name == pWarehouse.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }
                pWarehouse.Id = GuidUtil.New();
                //warehouse.Status = Enumeration.StoreStatus.Closed;
                pWarehouse.CreateTime = this.DateTime;
                pWarehouse.Creator = pOperater;
                CurrentDb.Warehouse.Add(pWarehouse);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

            }
            return result;
        }

        public CustomJsonResult Edit(string pOperater, Warehouse pWarehouse)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var lWarehouse = CurrentDb.Warehouse.Where(m => m.Id == pWarehouse.Id).FirstOrDefault();

                var isExistWarehouse = CurrentDb.Warehouse.Where(m => m.MerchantId == lWarehouse.MerchantId && m.Id != pWarehouse.Id && m.Name == pWarehouse.Name).FirstOrDefault();
                if (isExistWarehouse != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }



                lWarehouse.Name = pWarehouse.Name;
                lWarehouse.Address = pWarehouse.Address;
                lWarehouse.Description = pWarehouse.Description;
                // l_Warehouse.Status = store.Status;
                lWarehouse.MendTime = this.DateTime;
                lWarehouse.Mender = pOperater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }
    }
}
