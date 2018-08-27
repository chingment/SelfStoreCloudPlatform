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
        public CustomJsonResult Add(string operater, Warehouse warehouse)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var existObject = CurrentDb.Warehouse.Where(m => m.UserId == warehouse.UserId && m.Name == warehouse.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }
                warehouse.Id = GuidUtil.New();
                //warehouse.Status = Enumeration.StoreStatus.Closed;
                warehouse.CreateTime = this.DateTime;
                warehouse.Creator = operater;
                CurrentDb.Warehouse.Add(warehouse);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

            }
            return result;
        }

        public CustomJsonResult Edit(string operater, Warehouse warehouse)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var l_Warehouse = CurrentDb.Warehouse.Where(m => m.Id == warehouse.Id).FirstOrDefault();

                var existObject = CurrentDb.Warehouse.Where(m => m.UserId == l_Warehouse.UserId && m.Id != warehouse.Id && m.Name == warehouse.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }



                l_Warehouse.Name = warehouse.Name;
                l_Warehouse.Address = warehouse.Address;
                l_Warehouse.Description = warehouse.Description;
                // l_Warehouse.Status = store.Status;
                l_Warehouse.MendTime = this.DateTime;
                l_Warehouse.Mender = operater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }
    }
}
