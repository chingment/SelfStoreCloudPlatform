using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{


    public class CouponService : BaseProvider
    {
        public List<UserCouponModel> My(string operater, string clientId, RupCouponMy rup)
        {

            List<ClientCoupon> coupons;
            if (!rup.IsGetHis)
            {
                coupons = CurrentDb.ClientCoupon.Where(m => m.ClientId == clientId && m.Status == Entity.Enumeration.CouponStatus.WaitUse && m.EndTime > DateTime.Now).ToList();
            }
            else
            {
                coupons = CurrentDb.ClientCoupon.Where(m => m.ClientId == clientId && (m.Status == Entity.Enumeration.CouponStatus.Used || m.Status == Entity.Enumeration.CouponStatus.Expired) && m.EndTime < DateTime.Now).ToList();
            }

            var couponsModel = new List<UserCouponModel>();

            foreach (var item in coupons)
            {
                if (item.EndTime < DateTime.Now)
                {
                    item.Status = Entity.Enumeration.CouponStatus.Expired;
                    CurrentDb.SaveChanges();
                }

                var couponModel = new UserCouponModel();

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

                if (rup.CouponId != null)
                {
                    if (rup.CouponId.Contains(item.Id))
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
