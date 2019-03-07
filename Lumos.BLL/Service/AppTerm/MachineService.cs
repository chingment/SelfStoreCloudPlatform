using Lumos.BLL.Biz;
using Lumos.DAL;
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
        public CustomJsonResult InitData(RupMachineInitData rup)
        {
            CustomJsonResult result = new CustomJsonResult();

            var ret = new RetMachineInitData();

            var machine = CurrentDb.Machine.Where(m => m.Id == rup.MachineId).FirstOrDefault();

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

            ret.Machine.Id = machine.Id;
            ret.Machine.Name = machine.Name;
            ret.Machine.MerchantName = merchant.Name;
            ret.Machine.StoreName = store.Name;
            ret.Machine.PayTimeout = merchant.PayTimeout;
            ret.Machine.LogoImgUrl = machine.LogoImgUrl;

            ret.Banners = TermServiceFactory.Machine.GetBanners(machine.MerchantId, machine.StoreId, machine.Id);
            ret.ProductKinds = TermServiceFactory.ProductKind.GetKinds(machine.MerchantId, machine.StoreId, machine.Id);
            ret.ProductSkus = TermServiceFactory.Machine.GetProductSkus(machine.MerchantId, machine.StoreId, machine.Id);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }


        public Dictionary<string, ProductSkuModel> GetProductSkus(string merchantId, string storeId, string machineId)
        {

            var productSkuModels = new Dictionary<string, ProductSkuModel>();

            var machineStocks = CurrentDb.StoreSellStock.Where(m => m.MerchantId == merchantId && m.StoreId == storeId && m.ChannelId == machineId && m.IsOffSell == false).ToList();
            var productSkuIds = machineStocks.Select(m => m.ProductSkuId).Distinct();
            foreach (var productSkuId in productSkuIds)
            {
                var productSku = BizFactory.ProductSku.GetModel(productSkuId);
                if (productSku != null)
                {
                    var productSkuModel = new ProductSkuModel();
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

        public CustomJsonResult Login(RopMachineLogin rop)
        {

            var machine = CurrentDb.Machine.Where(m => m.Id == rop.MachineId).FirstOrDefault();

            if (machine == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败，该机器未登记");
            }

            var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.UserName == rop.UserName).FirstOrDefault();

            if (sysMerchantUser == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败,用户名或密码错误");
            }

            var isPasswordCorrect = PassWordHelper.VerifyHashedPassword(sysMerchantUser.PasswordHash, rop.Password);

            if (!isPasswordCorrect)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败,用户名或密码错误");
            }

            if (sysMerchantUser.MerchantId != machine.MerchantId)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "帐号与商户不对应");
            }

            var ret = new RetMachineLogin();

            ret.UserId = sysMerchantUser.Id;
            ret.UserName = sysMerchantUser.UserName;
            ret.FullName = sysMerchantUser.FullName;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "登录成功", ret);

        }
    }
}
