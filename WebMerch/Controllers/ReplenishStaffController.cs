using Lumos;
using WebMerch.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lumos.Entity;

namespace WebMerch.Controllers
{
    public class ReplenishStaffController : OwnBaseController
    {

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public CustomJsonResult GetList(SearchCondition condition)
        {
            string phoneNumber = condition.PhoneNumber.ToSearchString();


            var query = (from u in CurrentDb.MerchantReplenishStaff
                         join p in CurrentDb.SysUser on u.UserId equals p.Id
                         where (phoneNumber.Length == 0 || p.PhoneNumber.Contains(phoneNumber))
                         && u.MerchantId == this.CurrentUserId
                         select new { p.Id, p.Nickname, p.HeadImgUrl, p.PhoneNumber, u.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {
                olist.Add(new
                {
                    Id = item.Id,
                    Nickname = item.Nickname,
                    HeadImgUrl = item.HeadImgUrl,
                    PhoneNumber = item.PhoneNumber,
                    CreateTime = item.CreateTime
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }


        [HttpPost]
        public CustomJsonResult SearchStaff(SearchCondition condition)
        {
            string phoneNumber = condition.PhoneNumber.ToSearchString();

            var query = (from p in CurrentDb.SysUser
                         where (phoneNumber.Length > 3 && p.PhoneNumber == phoneNumber)
                         select new { p.Id, p.Nickname, p.HeadImgUrl, p.PhoneNumber, p.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {
                olist.Add(new
                {
                    Id = item.Id,
                    Nickname = item.Nickname,
                    HeadImgUrl = item.HeadImgUrl,
                    PhoneNumber = item.PhoneNumber,
                    CreateTime = item.CreateTime
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }


        [HttpPost]
        public CustomJsonResult Add(string userId)
        {
            var merchantReplenishStaff = CurrentDb.MerchantReplenishStaff.Where(m => m.MerchantId == this.CurrentUserId && m.UserId == userId).FirstOrDefault();

            if (merchantReplenishStaff != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "已经添加该用户");
            }

            merchantReplenishStaff = new MerchantReplenishStaff();
            merchantReplenishStaff.Id = GuidUtil.New();
            merchantReplenishStaff.MerchantId = this.CurrentUserId;
            merchantReplenishStaff.UserId = userId;
            merchantReplenishStaff.CreateTime = DateTime.Now;
            merchantReplenishStaff.Creator = this.CurrentUserId;
            CurrentDb.MerchantReplenishStaff.Add(merchantReplenishStaff);
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

        }

        [HttpPost]
        public CustomJsonResult Remove(string userId)
        {
            var merchantReplenishStaff = CurrentDb.MerchantReplenishStaff.Where(m => m.MerchantId == this.CurrentUserId && m.UserId == userId).FirstOrDefault();

            if (merchantReplenishStaff == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "已经被移除");
            }

            CurrentDb.MerchantReplenishStaff.Remove(merchantReplenishStaff);
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "移除成功");
        }

    }
}