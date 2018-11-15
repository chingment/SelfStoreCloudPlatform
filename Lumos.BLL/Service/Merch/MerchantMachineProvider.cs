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
    public class MerchantMachineProvider : BaseProvider
    {
        public CustomJsonResult Edit(string pOperater, string pMerchantId, RopMerchantMachineEdit rop)
        {
            var l_MerchantMachine = CurrentDb.MerchantMachine.Where(m => m.Id == rop.MerchantMachineId).FirstOrDefault();
            if (l_MerchantMachine != null)
            {
                //l_MerchantMachine.MachineName = rop.MachineName;
                l_MerchantMachine.Mender = pOperater;
                l_MerchantMachine.MendTime = DateTime.Now;
                CurrentDb.SaveChanges();
            }
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
        }

        public CustomJsonResult GetDetails(string pOperater, string pMerchantId, string pMerchantMachineId)
        {

            var ret = new RetMerchantMachineGetDetails();

            var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.Id == pMerchantMachineId).FirstOrDefault();
            if (merchantMachine != null)
            {
                var machine = CurrentDb.Machine.Where(m => m.Id == merchantMachine.MachineId).FirstOrDefault();
                var storeMachine = CurrentDb.StoreMachine.Where(m => m.MerchantId == merchantMachine.MerchantId && m.MachineId == merchantMachine.MachineId && m.IsBind == true).FirstOrDefault();
                if (storeMachine != null)
                {
                    ret.StoreId = storeMachine.StoreId;
                    ret.MachineId = merchantMachine.MachineId;
                    ret.DeviceId = machine.DeviceId;
                    ret.MachineName = storeMachine.MachineName;

                    var store = CurrentDb.Store.Where(m => m.Id == storeMachine.StoreId).FirstOrDefault();
                    if (store != null)
                    {
                        ret.StoreName = store.Name;

                        var skus = from u in CurrentDb.StoreSellStock
                                   where
                                   u.MerchantId == pMerchantId &&
                                   u.StoreId == ret.StoreId &&
                                   u.ChannelId == ret.MachineId &&
                                   u.ChannelType == Enumeration.ChannelType.Machine
                                   select new { u.Id, u.SlotId, u.ProductSkuId, u.ProductSkuName, u.Quantity, u.LockQuantity, u.SellQuantity, u.SalePrice, u.SalePriceByVip };

                        List<object> olist = new List<object>();
                        foreach (var item in skus)
                        {
                            var skuModel = BizFactory.ProductSku.GetModel(item.ProductSkuId);
                            if (skuModel != null)
                            {
                                ret.Skus.Add(new RetMerchantMachineGetDetails.SkuModel
                                {
                                    SlotId = item.SlotId,
                                    SkuId = skuModel.Id,
                                    SkuName = skuModel.Name,
                                    SkuImgUrl = skuModel.ImgUrl,
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
            }
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }
    }
}
