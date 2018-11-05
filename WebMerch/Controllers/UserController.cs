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
    public class UserController : OwnBaseController
    {

        public ActionResult ListByReplenishStaff()
        {
            return View();
        }



        [HttpPost]
        public CustomJsonResult GetListByReplenishStaff(SearchCondition condition)
        {
            string phoneNumber = condition.PhoneNumber.ToSearchString();


            var query = (from u in CurrentDb.MerchantReplenishStaff
                         join p in CurrentDb.SysUser on u.UserId equals p.Id
                         where (phoneNumber.Length == 0 || p.PhoneNumber.Contains(phoneNumber))
                         && u.MerchantId == this.CurrentUserId
                         select new { u.Id, p.Nickname, p.HeadImgUrl, p.PhoneNumber, u.CreateTime });

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

    }
}