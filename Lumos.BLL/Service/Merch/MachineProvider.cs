using Lumos.BLL.Biz;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Merch
{
    public class MachineProvider : BaseProvider
    {
        public CustomJsonResult Edit(string operater, string merchantId, RopMachineEdit rop)
        {
            var machine = CurrentDb.Machine.Where(m => m.MerchantId == merchantId && m.Id == rop.Id).FirstOrDefault();
            if (machine != null)
            {
                //l_MerchantMachine.ma = rop.Name;
                machine.Mender = operater;
                machine.MendTime = DateTime.Now;
                CurrentDb.SaveChanges();
            }
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public CustomJsonResult GetDetails(string operater, string merchantId, string id)
        {

            var ret = new RetMachineGetDetails();


            var machine = CurrentDb.Machine.Where(m => m.Id == id).FirstOrDefault();
            var storeMachine = CurrentDb.StoreMachine.Where(m => m.MerchantId == merchantId && m.MachineId == id && m.IsBind == true).FirstOrDefault();
            if (storeMachine != null)
            {
                ret.Id = id;
                ret.DeviceId = machine.DeviceId;
                ret.Name = storeMachine.MachineName;

                var store = CurrentDb.Store.Where(m => m.Id == storeMachine.StoreId).FirstOrDefault();
                if (store != null)
                {
                    ret.Store.Id = store.Id;
                    ret.Store.Name = store.Name;
                    ret.Store.Address = store.Address;

                    var skus = from u in CurrentDb.StoreSellStock
                               where
                               u.MerchantId == merchantId &&
                               u.StoreId == store.Id &&
                               u.ChannelId == id &&
                               u.ChannelType == Enumeration.ChannelType.Machine
                               select new { u.Id, u.SlotId, u.ProductSkuId, u.ProductSkuName, u.Quantity, u.LockQuantity, u.SellQuantity, u.SalePrice, u.SalePriceByVip };

                    List<object> olist = new List<object>();
                    foreach (var item in skus)
                    {
                        var skuModel = BizFactory.ProductSku.GetModel(item.ProductSkuId);
                        if (skuModel != null)
                        {
                            ret.Skus.Add(new RetMachineGetDetails.SkuModel
                            {
                                Id = skuModel.Id,
                                Name = skuModel.Name,
                                ImgUrl = skuModel.ImgUrl,
                                SlotId = item.SlotId,
                                Quantity = item.Quantity,
                                LockQuantity = item.LockQuantity,
                                SellQuantity = item.SellQuantity,
                                SalePrice = item.SalePrice,
                                SalePriceByVip = item.SalePriceByVip
                            });
                        }
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }
    }
}
