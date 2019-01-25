using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Lumos.Entity;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Common;
using Lumos.Web.Mvc;
using Lumos.DAL;
using Lumos.BLL;
using Lumos;
using Lumos.BLL.Service.Merch;
using System.Collections.Generic;

namespace WebMerch.Controllers
{
    public class UserController : OwnBaseController
    {

        #region 视图

        public ViewResult List()
        {
            return View();
        }

        public ViewResult ListBySelect()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        public ViewResult Edit()
        {
            return View();
        }
        #endregion

        public CustomJsonResult GetList(RupUserGetList rup)
        {
            var query = (from u in CurrentDb.SysMerchantUser
                         where (rup.UserName == null || u.UserName.Contains(rup.UserName)) &&
                         (rup.FullName == null || u.FullName.Contains(rup.FullName)) &&
                         u.IsDelete == false &&
                         u.IsCanDelete == true &&
                         u.MerchantId == this.CurrentMerchantId
                         select new { u.Id, u.UserName, u.FullName, u.PositionId, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete, u.Status });


            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {

                olist.Add(new
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    FullName = item.FullName,
                    Email = item.Email,
                    PositionName = item.PositionId.GetCnName(),
                    PhoneNumber = item.PhoneNumber,
                    StatusName = item.Status.GetCnName(),
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime()
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        public CustomJsonResult GetDetails(string id)
        {
            return MerchServiceFactory.User.GetDetails(this.CurrentUserId, this.CurrentMerchantId, id);
        }

        public CustomJsonResult InitDataToAdd()
        {
            var merchant = CurrentDb.Merchant.Where(m => m.Id == this.CurrentMerchantId).FirstOrDefault();
            var data = new { simpleCode = merchant.SimpleCode };
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "", data);
        }

        [HttpPost]
        public CustomJsonResult Add(RopUserAdd rop)
        {
            return MerchServiceFactory.User.Add(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopUserEdit rop)
        {
            return MerchServiceFactory.User.Edit(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(string id)
        {
            return MerchServiceFactory.User.Delete(this.CurrentUserId, this.CurrentMerchantId, id);
        }

    }
}