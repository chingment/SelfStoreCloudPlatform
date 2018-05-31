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

        public ViewResult Edit(int id)
        {
            EditViewModel model = new EditViewModel(id);
            return View(model);
        }

        public CustomJsonResult GetList(SearchCondition condition)
        {
            string deviceId = condition.DeviceId.ToSearchString();
            string macAddress = condition.MacAddress.ToSearchString();

            var list = (from p in CurrentDb.Machine
                        where
                                (deviceId.Length == 0 || p.DeviceId.Contains(deviceId)) &&
                                 (macAddress.Length == 0 || p.MacAddress.Contains(macAddress))
                        select new { p.Id, p.Name, p.DeviceId, p.MacAddress, p.CreateTime });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);


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