using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{


    public class CouponService : BaseProvider
    {
        public List<CouponModel> List(int operater, int userId, bool isGetHis, List<int> couponId, List<OrderConfirmSkuModel> skus)
        {

            List<Coupon> coupons;
            if (!isGetHis)
            {
                coupons = CurrentDb.Coupon.Where(m => m.UserId == userId && m.Status == Entity.Enumeration.CouponStatus.WaitUse && m.EndTime > DateTime.Now).ToList();
            }
            else
            {
                coupons = CurrentDb.Coupon.Where(m => m.UserId == userId && (m.Status == Entity.Enumeration.CouponStatus.Used || m.Status == Entity.Enumeration.CouponStatus.Expired) && m.EndTime < DateTime.Now).ToList();
            }

            var couponsModel = new List<CouponModel>();

            foreach (var item in coupons)
            {
                if (item.EndTime < DateTime.Now)
                {
                    item.Status = Entity.Enumeration.CouponStatus.Expired;
                    CurrentDb.SaveChanges();
                }

                var couponModel = new CouponModel();

                couponModel.Id = item.Id;
                couponModel.Name = item.Name;
                switch (item.Type)
                {
                    case Entity.Enumeration.CouponType.FullCut:
                    case Entity.Enumeration.CouponType.UnLimitedCut:
                        couponModel.Discount = item.Discount.ToF2Price();
                        couponModel.DiscountUnit = "元";
                        couponModel.DiscountTip = "满减卷";
                        break;
                    case Entity.Enumeration.CouponType.Discount:
                        couponModel.Discount = item.Discount.ToF2Price();
                        couponModel.DiscountUnit = "折";
                        couponModel.DiscountTip = "折扣卷";
                        break;
                }

                couponModel.ValidDate = "有效到" + item.EndTime.ToUnifiedFormatDate();
                couponModel.Description = "全场使用";

                if (couponId != null)
                {
                    if (couponId.Contains(item.Id))
                    {
                        couponModel.IsSelected = true;
                    }
                }

                couponsModel.Add(couponModel);
            }

            return couponsModel;
        }
    }
}
