using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Sys.Banner;

namespace WebBack.Controllers.Sys
{
    public class BannerController : OwnBaseController
    {
        // GET: Banner
        public ViewResult BannerList()
        {
            return View();
        }

        public ViewResult TypeList()
        {
            return View();
        }

        public ViewResult AddBanner()
        {
            return View();
        }

        public ViewResult BannerDetails(int id)
        {
            BannerDetailsViewModel model = new BannerDetailsViewModel(id);
            if (id == 0)
            {
                model.SysBanner.CreateTime = DateTime.Now;
            }
            return View(model);
        }


        public ViewResult EditBanner(int id)
        {
            EditBannerViewModel model = new EditBannerViewModel(id);
            return View(model);
        }

        public CustomJsonResult GetBannerList(SearchCondition condition)
        {
            var query = (from u in CurrentDb.SysBanner
                         where u.Type == condition.Type
                         select new { u.Id, u.Title, u.ImgUrl, u.Status, u.ReadCount, u.CreateTime, u.LinkUrl });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {
                list.Add(new
                {
                    Id = item.Id,
                    Title = item.Title,
                    ImgUrl = item.ImgUrl,
                    ReadCount = item.ReadCount,
                    StatusName = item.Status.GetCnName(),
                    Status = item.Status,
                    LinkUrl = SysFactory.Banner.GetLinkUrl(item.Id),
                    CreateTime = item.CreateTime

                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        public CustomJsonResult GetTypeList(SearchCondition condition)
        {
            var list = (from u in CurrentDb.SysBannerType

                        select new { u.Id, u.Name, u.Description, u.CreateTime });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        [HttpPost]
        public CustomJsonResult AddBanner(AddBannerViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = SysFactory.Banner.Add(this.CurrentUserId, model.Operate, model.SysBanner);

            return reuslt;
        }


        [HttpPost]
        public CustomJsonResult EditBanner(EditBannerViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = SysFactory.Banner.Edit(this.CurrentUserId, model.Operate, model.SysBanner);

            return reuslt;
        }
    }
}