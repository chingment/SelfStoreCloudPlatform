using Lumos.BLL.Service.Term.Models;
using Lumos.Entity;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class MachineService : BaseProvider
    {

        //private void test()
        //{

        //    string useriId = "ca66ca85c5bf435581ecd2380554ecfe";
        //    //string merchantId = "d1e8ad564c0f4516b2de95655a4146c7";
        //    string machineId = "00000000000000000000000000000006";
        //    string storeId = "00000000000000000000000000000006";
        //    var machineStocks = CurrentDb.MachineStock.Where(m => m.MerchantId == useriId  && m.MachineId == machineId).ToList();

        //    foreach (var item in machineStocks)
        //    {
        //        CurrentDb.MachineStock.Remove(item);
        //        CurrentDb.SaveChanges();
        //    }


        //    var productSkus = CurrentDb.ProductSku.ToList();

        //    foreach (var item in productSkus)
        //    {
        //        var machineStock = new MachineStock();

        //        machineStock.Id = GuidUtil.New();
        //        machineStock.UserId = useriId;
        //        machineStock.MachineId = machineId;
        //        machineStock.StoreId = storeId;
        //        machineStock.SlotId = GuidUtil.New();
        //        machineStock.ProductSkuId = item.Id;
        //        machineStock.Quantity = 2;
        //        machineStock.SellQuantity = 1;
        //        machineStock.LockQuantity = 1;
        //        machineStock.IsOffSell = false;
        //        machineStock.CreateTime = DateTime.Now;
        //        machineStock.Creator = machineId;

        //        CurrentDb.MachineStock.Add(machineStock);
        //        CurrentDb.SaveChanges();

        //    }

        //}

        public CustomJsonResult ApiConfig(string pOperater, string pDeviceId)
        {
            CustomJsonResult result = new CustomJsonResult();

            var machine = CurrentDb.Machine.Where(m => m.DeviceId == pDeviceId).FirstOrDefault();

            if (machine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未入库登记");
            }

            if (!machine.IsUse)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未配置商户");
            }


            var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MachineId == machine.Id & m.IsBind == true).FirstOrDefault();

            if (merchantMachine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未绑定商户");
            }

            var merchantConfig = CurrentDb.MerchantConfig.Where(m => m.Id == merchantMachine.MerchantId).FirstOrDefault();
            if (merchantConfig == null)
            {
                return new CustomJsonResult(ResultType.Failure, "已绑定商户，却找不到商户信息");
            }

            var storeMachine = CurrentDb.StoreMachine.Where(m => m.MerchantId == merchantMachine.MerchantId && m.MachineId == merchantMachine.MachineId && m.IsBind == true).FirstOrDefault();

            if (storeMachine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未绑定店铺");
            }

            var merchant = CurrentDb.SysMerchantUser.Where(m => m.Id == storeMachine.MerchantId).FirstOrDefault();

            if (merchant == null)
            {
                return new CustomJsonResult(ResultType.Failure, "商户不存在");
            }

            var store = CurrentDb.Store.Where(m => m.Id == storeMachine.StoreId).FirstOrDefault();

            if (store == null)
            {
                return new CustomJsonResult(ResultType.Failure, "店铺不存在");
            }

            var ret = new RetMachineApiConfig();
            ret.MerchantId = storeMachine.MerchantId;
            ret.MerchantName = merchant.MerchantName;
            ret.StoreId = storeMachine.StoreId;
            ret.StoreName = store.Name;
            ret.MachineId = storeMachine.MachineId;
            ret.ApiHost = merchantConfig.ApiHost;
            ret.ApiKey = merchantConfig.ApiKey;
            ret.ApiSecret = merchantConfig.ApiSecret;
            ret.PayTimeout = merchantConfig.PayTimeout;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public Dictionary<string, ProductSkuModel> GetProductSkus(string pOperater, string pMerchantId, string pMachineId)
        {

            var productSkuModels = new Dictionary<string, ProductSkuModel>();

            var machineStocks = CurrentDb.StoreSellStock.Where(m => m.MerchantId == pMerchantId && m.ChannelId == pMachineId && m.IsOffSell == false).ToList();

            var productSkus = CurrentDb.ProductSku.Where(m => m.MerchantId == pMerchantId).ToList();
            var productSkuIds = machineStocks.Select(m => m.ProductSkuId).Distinct();
            foreach (var productSkuId in productSkuIds)
            {
                var productSku = productSkus.Where(m => m.Id == productSkuId).FirstOrDefault();
                if (productSku != null)
                {
                    var productSkuModel = new ProductSkuModel();

                    productSkuModel.Quantity = machineStocks.Where(m => m.ProductSkuId == productSkuId).Sum(m => m.Quantity);
                    productSkuModel.LockQuantity = machineStocks.Where(m => m.ProductSkuId == productSkuId).Sum(m => m.LockQuantity);
                    productSkuModel.SellQuantity = machineStocks.Where(m => m.ProductSkuId == productSkuId).Sum(m => m.SellQuantity);

                    productSkuModel.Id = productSku.Id;
                    productSkuModel.Name = productSku.Name;
                    productSkuModel.SpecDes = productSku.SpecDes;
                    productSkuModel.DetailsDes = productSku.DetailsDes;
                    productSkuModel.BriefInfo = productSku.BriefInfo;
                    productSkuModel.ShowPirce = productSku.ShowPrice.ToF2Price();
                    productSkuModel.SalePrice = productSku.SalePrice.ToF2Price();
                    productSkuModel.DisplayImgUrls = productSku.DispalyImgUrls;
                    productSkuModel.ImgUrl = ImgSet.GetMain(productSku.DispalyImgUrls);

                    productSkuModels.Add(productSku.Id, productSkuModel);
                }
            }


            return productSkuModels;
        }

        public List<BannerModel> GetBanners(string pOperater, string pMerchantId, string machineId)
        {
            var bannerModels = new List<BannerModel>();

            var banners = CurrentDb.MachineBanner.Where(m => m.MerchantId == pMerchantId && m.MachineId == machineId && m.Status == Entity.Enumeration.MachineBannerStatus.Normal).OrderByDescending(m => m.Priority).ToList();

            foreach (var item in banners)
            {

                bannerModels.Add(new BannerModel { ImgUrl = item.ImgUrl });
            }

            return bannerModels;
        }

        public CustomJsonResult GetSlotSkusStock(string pOperater, string pMerchantId, string pMachineId)
        {
            var slotProductSkuModels = new List<SlotProductSkuModel>();

            var machineStocks = CurrentDb.StoreSellStock.Where(m => m.MerchantId == pMerchantId && m.ChannelId == pMachineId && m.IsOffSell == false).ToList();

            var productSkus = CurrentDb.ProductSku.Where(m => m.MerchantId == pMerchantId).ToList();

            foreach (var item in machineStocks)
            {
                var productSku = productSkus.Where(m => m.Id == item.ProductSkuId).FirstOrDefault();
                if (productSku != null)
                {
                    var slotProductSkuModel = new SlotProductSkuModel();

                    slotProductSkuModel.Id = productSku.Id;
                    slotProductSkuModel.SlotId = item.SlotId;
                    slotProductSkuModel.Name = productSku.Name;
                    slotProductSkuModel.ImgUrl = "";
                    slotProductSkuModel.Quantity = item.Quantity;
                    slotProductSkuModel.LockQuantity = item.LockQuantity;
                    slotProductSkuModel.SellQuantity = item.SellQuantity;

                    slotProductSkuModels.Add(slotProductSkuModel);
                }
            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", slotProductSkuModels);
        }

        public CustomJsonResult UpdateInfo(string pOperater, RopMachineUpdateInfo rop)
        {
            var result = new CustomJsonResult();

            var storeMachine = CurrentDb.StoreMachine.Where(m => m.MerchantId == rop.MerchantId && m.StoreId == rop.StoreId && m.MachineId == rop.MachineId).FirstOrDefault();

            if (storeMachine == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "更新失败，找不到机器信息");
            }

            if (rop.Lat > 0)
            {
                storeMachine.Lat = rop.Lat;
            }

            if (rop.Lng > 0)
            {
                storeMachine.Lng = rop.Lng;
            }

            if (string.IsNullOrEmpty(rop.JPushRegId))
            {
                storeMachine.JPushRegId = rop.JPushRegId;
            }

            storeMachine.Mender = pOperater;
            storeMachine.MendTime = this.DateTime;

            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "更新成功");
        }


        public CustomJsonResult LoginResultQuery(string pOperater, RupMachineLoginResultQuery rup)
        {

            CustomJsonResult result = new CustomJsonResult();

            Biz.RModels.RupMachineLoginResultQuery bizRup = new Biz.RModels.RupMachineLoginResultQuery();

            bizRup.Token = rup.Token;

            result = BizFactory.Machine.LoginResultQuery(pOperater, bizRup);

            return result;
        }

    }
}
