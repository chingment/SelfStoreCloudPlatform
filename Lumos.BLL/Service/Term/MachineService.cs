using Lumos.BLL.Service.Term.Models;
using Lumos.BLL.Service.Term.Models.Machine;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class MachineService : BaseProvider
    {

        private void test()
        {

            string useriId = "ca66ca85c5bf435581ecd2380554ecfe";
            string merchantId = "d1e8ad564c0f4516b2de95655a4146c7";
            string machineId = "00000000000000000000000000000006";
            string storeId = "00000000000000000000000000000006";
            var machineStocks = CurrentDb.MachineStock.Where(m => m.UserId == useriId && m.MerchantId == merchantId && m.MachineId == machineId).ToList();

            foreach (var item in machineStocks)
            {
                CurrentDb.MachineStock.Remove(item);
                CurrentDb.SaveChanges();
            }


            var productSkus = CurrentDb.ProductSku.ToList();

            foreach (var item in productSkus)
            {
                var machineStock = new MachineStock();

                machineStock.Id = GuidUtil.New();
                machineStock.UserId = useriId;
                machineStock.MerchantId = merchantId;
                machineStock.MachineId = machineId;
                machineStock.StoreId = storeId;
                machineStock.SlotId = GuidUtil.New();
                machineStock.ProductSkuId = item.Id;
                machineStock.Quantity = 2;
                machineStock.SellQuantity = 1;
                machineStock.LockQuantity = 1;
                machineStock.IsOffSell = false;
                machineStock.CreateTime = DateTime.Now;
                machineStock.Creator = machineId;

                CurrentDb.MachineStock.Add(machineStock);
                CurrentDb.SaveChanges();

            }

        }

        public CustomJsonResult ApiConfig(string operater, string deviceId)
        {
            CustomJsonResult result = new CustomJsonResult();

            var machine = CurrentDb.Machine.Where(m => m.DeviceId == deviceId).FirstOrDefault();

            if (machine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未入库登记");
            }

            if (!machine.IsUse)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未配置商户");
            }


            var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MachineId == machine.Id & m.Status == Entity.Enumeration.MerchantMachineStatus.Bind).FirstOrDefault();

            if (merchantMachine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未绑定商户");
            }

            var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantMachine.MerchantId).FirstOrDefault();
            if (merchant == null)
            {
                return new CustomJsonResult(ResultType.Failure, "已绑定商户，却找不到商户信息");
            }

            var model = new ApiConfigModel();
            model.UserId = merchant.UserId;
            model.MerchantId = merchant.Id;
            model.MachineId = machine.Id;
            model.ApiHost = merchant.ApiHost;
            model.ApiKey = merchant.ApiKey;
            model.ApiSecret = merchant.ApiSecret;
            model.PayTimeout = merchant.PayTimeout;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", model);
        }

        public Dictionary<string, ProductSkuModel> GetProductSkus(string operater, string userId, string merchantId, string machineId)
        {

            var productSkuModels = new Dictionary<string, ProductSkuModel>();

            var machineStocks = CurrentDb.MachineStock.Where(m => m.UserId == userId && m.MerchantId == merchantId && m.MachineId == machineId && m.IsOffSell == false).ToList();

            var productSkus = CurrentDb.ProductSku.Where(m => m.UserId == userId && m.MerchantId == merchantId).ToList();
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
                    productSkuModel.KindId = productSku.KindId;
                    productSkuModel.SpecDes = productSku.SpecDes;
                    productSkuModel.DetailsDes = productSku.DetailsDes;
                    productSkuModel.BriefInfo = productSku.BriefInfo;
                    productSkuModel.ShowPirce = productSku.ShowPrice.ToF2Price();
                    productSkuModel.SalesPrice = productSku.SalePrice.ToF2Price();
                    productSkuModel.DisplayImgUrls = productSku.DispalyImgUrls;
                    productSkuModel.ImgUrl = ImgSet.GetMain(productSku.DispalyImgUrls);

                    productSkuModels.Add(productSku.Id,productSkuModel);
                }
            }


            return productSkuModels;
        }

        public List<BannerModel> GetBanners(string operater, string userId, string merchantId, string machineId)
        {
            var bannerModels = new List<BannerModel>();

            var banners = CurrentDb.MachineBanner.Where(m => m.MerchantId == merchantId && m.MachineId == machineId && m.Status == Entity.Enumeration.MachineBannerStatus.Normal).OrderByDescending(m => m.Priority).ToList();

            foreach (var item in banners)
            {

                bannerModels.Add(new BannerModel { ImgUrl = item.ImgUrl });
            }

            return bannerModels;
        }

        public CustomJsonResult GetSlotSkusStock(string operater, string userId, string merchantId, string machineId)
        {
            var slotProductSkuModels = new List<SlotProductSkuModel>();

            var machineStocks = CurrentDb.MachineStock.Where(m => m.UserId == userId && m.MerchantId == merchantId && m.MachineId == machineId && m.IsOffSell == false).ToList();

            var productSkus = CurrentDb.ProductSku.Where(m => m.UserId == userId && m.MerchantId == merchantId).ToList();

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
    }
}
