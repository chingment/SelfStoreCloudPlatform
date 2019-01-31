using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Merch
{
    public class WarehouseProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string merchantId, string id)
        {
            var ret = new RetWarehouseGetDetails();
            var warehouse = CurrentDb.ImsWarehouse.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
            if (warehouse != null)
            {
                ret.Id = warehouse.Id ?? "";
                ret.Name = warehouse.Name ?? "";
                ret.Address = warehouse.Address ?? "";
                ret.Description = warehouse.Description ?? "";
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult Add(string operater, string merchantId, RopWarehouseAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var existObject = CurrentDb.ImsWarehouse.Where(m => m.MerchantId == merchantId && m.Name == rop.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }
                var warehouse = new ImsWarehouse();
                warehouse.Id = GuidUtil.New();
                warehouse.MerchantId = merchantId;
                warehouse.Name = rop.Name;
                warehouse.Address = rop.Address;
                warehouse.Description = rop.Description;
                warehouse.CreateTime = this.DateTime;
                warehouse.Creator = operater;
                CurrentDb.ImsWarehouse.Add(warehouse);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

            }
            return result;
        }

        public CustomJsonResult Edit(string operater, string merchantId, RopWarehouseEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var warehouse = CurrentDb.ImsWarehouse.Where(m => m.Id == rop.Id && m.MerchantId == merchantId).FirstOrDefault();

                var isExistWarehouse = CurrentDb.ImsWarehouse.Where(m => m.MerchantId == merchantId && m.Id != warehouse.Id && m.Name == rop.Name).FirstOrDefault();
                if (isExistWarehouse != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }

                warehouse.Name = rop.Name;
                warehouse.Address = rop.Address;
                warehouse.Description = rop.Description;
                warehouse.MendTime = this.DateTime;
                warehouse.Mender = operater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }
            return result;
        }
    }
}
