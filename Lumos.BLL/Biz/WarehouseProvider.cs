using Lumos.BLL.Biz;
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
        public CustomJsonResult GetDetails(string pOperater, string pMerchantId, string pWarehouseId)
        {
            var ret = new RetWarehouseGetDetails();
            var warehouse = CurrentDb.Warehouse.Where(m => m.MerchantId == pMerchantId && m.Id == pWarehouseId).FirstOrDefault();
            if (warehouse != null)
            {
                ret.WarehouseId = warehouse.Id ?? "";
                ret.Name = warehouse.Name ?? "";
                ret.Address = warehouse.Address ?? "";
                ret.Description = warehouse.Description ?? "";
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string pOperater, string pMerchantId, RopWarehouseAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var existObject = CurrentDb.Warehouse.Where(m => m.MerchantId == pMerchantId && m.Name == rop.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }
                var warehouse = new Warehouse();
                warehouse.Id = GuidUtil.New();
                warehouse.MerchantId = pMerchantId;
                warehouse.Name = rop.Name;
                warehouse.Address = rop.Address;
                warehouse.Description = rop.Description;
                warehouse.CreateTime = this.DateTime;
                warehouse.Creator = pOperater;
                CurrentDb.Warehouse.Add(warehouse);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

            }
            return result;
        }

        public CustomJsonResult Edit(string pOperater, string pMerchantId, RopWarehouseEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var warehouse = CurrentDb.Warehouse.Where(m => m.Id == rop.WarehouseId && m.MerchantId == pMerchantId).FirstOrDefault();

                var isExistWarehouse = CurrentDb.Warehouse.Where(m => m.MerchantId == pMerchantId && m.Id != warehouse.Id && m.Name == rop.Name).FirstOrDefault();
                if (isExistWarehouse != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }

                warehouse.Name = rop.Name;
                warehouse.Address = rop.Address;
                warehouse.Description = rop.Description;
                warehouse.MendTime = this.DateTime;
                warehouse.Mender = pOperater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }
    }
}
