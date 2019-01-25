using Lumos.BLL.Biz;
using Lumos.BLL.Service.AppMobile;
using Lumos.Entity;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public class MachineService : BaseService
    {
        public CustomJsonResult ApiConfig(RupMachineApiConfig rup)
        {
            CustomJsonResult result = new CustomJsonResult();

            var machine = CurrentDb.Machine.Where(m => m.Id == rup.DeviceId).FirstOrDefault();

            if (machine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未入库登记");
            }

            if (string.IsNullOrEmpty(machine.MerchantId))
            {
                return new CustomJsonResult(ResultType.Failure, "未绑定商户");
            }

            var merchant = CurrentDb.Merchant.Where(m => m.Id == machine.MerchantId).FirstOrDefault();

            if (merchant == null)
            {
                return new CustomJsonResult(ResultType.Failure, "商户不存在");
            }

            if (string.IsNullOrEmpty(machine.StoreId))
            {
                return new CustomJsonResult(ResultType.Failure, "未绑定店铺");
            }

            var store = CurrentDb.Store.Where(m => m.Id == machine.StoreId).FirstOrDefault();

            if (store == null)
            {
                return new CustomJsonResult(ResultType.Failure, "店铺不存在");
            }

            var ret = new RetMachineApiConfig();
            ret.MachineId = machine.Id;
            ret.MachineName = machine.Name;
            ret.MerchantName = merchant.Name;
            ret.StoreName = store.Name;
            ret.ApiHost = merchant.ApiHost;
            ret.ApiKey = merchant.ApiKey;
            ret.ApiSecret = merchant.ApiSecret;
            ret.PayTimeout = merchant.PayTimeout;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public Dictionary<string, ProductSkuModel> GetProductSkus(string merchantId, string storeId, string machineId)
        {

            var productSkuModels = new Dictionary<string, ProductSkuModel>();

            var machineStocks = CurrentDb.StoreSellStock.Where(m => m.MerchantId == merchantId && m.ChannelId == machineId && m.IsOffSell == false).ToList();

            var productSkus = CurrentDb.ProductSku.Where(m => m.MerchantId == merchantId).ToList();
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

        public List<BannerModel> GetBanners(string merchantId, string storeId, string machineId)
        {
            var bannerModels = new List<BannerModel>();

            var banners = CurrentDb.MachineBanner.Where(m => m.MerchantId == merchantId && m.StoreId == storeId && m.MachineId == machineId && m.Status == Entity.Enumeration.MachineBannerStatus.Normal).OrderByDescending(m => m.Priority).ToList();

            foreach (var item in banners)
            {
                bannerModels.Add(new BannerModel { ImgUrl = item.ImgUrl });
            }

            return bannerModels;
        }

        public CustomJsonResult GetSlotSkusStock(string merchantId, string machineId)
        {
            var slotProductSkuModels = new List<SlotProductSkuModel>();

            var machineStocks = CurrentDb.StoreSellStock.Where(m => m.MerchantId == merchantId && m.ChannelId == machineId && m.IsOffSell == false).ToList();

            var productSkus = CurrentDb.ProductSku.Where(m => m.MerchantId == merchantId).ToList();

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


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", slotProductSkuModels);
        }

        public CustomJsonResult UpdateInfo(RopMachineUpdateInfo rop)
        {
            var result = new CustomJsonResult();

            var machine = CurrentDb.Machine.Where(m => m.Id == rop.MachineId).FirstOrDefault();

            if (machine == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "更新失败，找不到机器信息");
            }

            if (rop.Lat > 0)
            {
                machine.Lat = rop.Lat;
            }

            if (rop.Lng > 0)
            {
                machine.Lng = rop.Lng;
            }

            if (string.IsNullOrEmpty(rop.JPushRegId))
            {
                machine.JPushRegId = rop.JPushRegId;
            }


            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "更新成功");
        }

        public CustomJsonResult LoginResultQuery(RupMachineLoginResultQuery rup)
        {
            var key = string.Format("machineLoginResult:{0}", rup.Token);

            var redis = new RedisClient<string>();
            var token = redis.KGetString(key);

            if (token == null)
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败");


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "登录成功");
        }

        public CustomJsonResult LoginByQrCode(RopMachineLoginByQrCode rop)
        {

            var ret = new RetOperateResult();

            var key = string.Format("machineLoginResult:{0}", rop.Token.ToLower());
            var redis = new RedisClient<string>();
            var isFlag = redis.KSet(key, "true", new TimeSpan(0, 1, 0));
            if (!isFlag)
            {
                ret.Result = RetOperateResult.ResultType.Success;
                ret.Remarks = "";
                ret.Message = "登录失败";
                ret.IsComplete = true;
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败", ret);
            }
            else
            {
                ret.Result = RetOperateResult.ResultType.Success;
                ret.Remarks = "";
                ret.Message = "登录成功";
                ret.IsComplete = true;
                return new CustomJsonResult(ResultType.Success, ResultCode.Success, "登录成功", ret);
            }
        }
    }
}
