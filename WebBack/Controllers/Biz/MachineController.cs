using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.BLL;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Transactions;
using WebBack.Models.Biz.Machine;
using Lumos;

namespace WebBack.Controllers.Biz
{
    public class MachineController : OwnBaseController
    {

        public ViewResult List()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        public ViewResult Edit(string id)
        {
            EditViewModel model = new EditViewModel(id);
            return View(model);
        }

        public CustomJsonResult GetList(SearchCondition condition)
        {
            string deviceId = condition.DeviceId.ToSearchString();

            var query = (from p in CurrentDb.Machine
                         where
                                 (deviceId.Length == 0 || p.DeviceId.Contains(deviceId))
                         select new { p.Id, p.Name, p.DeviceId, p.MacAddress, p.IsUse, p.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {

                list.Add(new
                {
                    item.Id,
                    item.Name,
                    item.DeviceId,
                    IsUse = (item.IsUse == true ? "是" : "否"),
                    item.MacAddress,
                    item.CreateTime
                });


            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        [HttpPost]
        public CustomJsonResult Add(AddViewModel model)
        {
            return BizFactory.Machine.Add(this.CurrentUserId, model.Machine);
        }


        [HttpPost]

        public CustomJsonResult Edit(EditViewModel model)
        {
            return BizFactory.Machine.Edit(this.CurrentUserId, model.Machine);
        }

    }
}