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
    public class MerchantMachineProvider : BaseProvider
    {
        public CustomJsonResult BindOn(string pOperater, string pMerchantId, string pMachineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMachine = CurrentDb.Machine.Where(m => m.Id == pMachineId).FirstOrDefault();

                if (lMachine.IsUse)
                {
                    return new CustomJsonResult(ResultType.Failure, "该设备已经被绑定，未被解绑");
                }

                lMachine.IsUse = true;
                lMachine.MendTime = this.DateTime;
                lMachine.Mender = pOperater;

                var lMerchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == pMerchantId && m.MachineId == pMachineId).FirstOrDefault();
                if (lMerchantMachine == null)
                {
                    lMerchantMachine = new MerchantMachine();
                    lMerchantMachine.Id = GuidUtil.New();
                    lMerchantMachine.MerchantId = pMerchantId;
                    lMerchantMachine.MachineId = pMachineId;
                    lMerchantMachine.IsBind = true;
                    lMerchantMachine.CreateTime = this.DateTime;
                    lMerchantMachine.Creator = pOperater;
                    CurrentDb.MerchantMachine.Add(lMerchantMachine);
                }
                else
                {
                    lMerchantMachine.IsBind = true;
                    lMerchantMachine.MendTime = this.DateTime;
                    lMerchantMachine.Mender = pOperater;
                }

                lMerchantMachine.LogoImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/LogoImg.png";
                lMerchantMachine.BtnBuyImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/BtnBuyImg.png";
                lMerchantMachine.BtnPickImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/BtnPickImg.png";

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "绑定成功");
            }

            return result;
        }

        public CustomJsonResult BindOff(string pOperater, string pMerchantId, string pMachineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMerchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == pMerchantId && m.MachineId == pMachineId).FirstOrDefault();

                if (lMerchantMachine.IsBind == false)
                {

                    return new CustomJsonResult(ResultType.Failure, "该设备已经被解绑");
                }

                lMerchantMachine.IsBind = false;
                lMerchantMachine.MendTime = this.DateTime;
                lMerchantMachine.Mender = pOperater;

                var machine = CurrentDb.Machine.Where(m => m.Id == pMachineId).FirstOrDefault();

                machine.IsUse = false;
                machine.MendTime = this.DateTime;
                machine.Mender = pOperater;

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "解绑成功");
            }

            return result;
        }

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
