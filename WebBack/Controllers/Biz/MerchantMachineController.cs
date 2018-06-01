using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.MerchantMachine;

namespace WebBack.Controllers.Biz
{
    public class MerchantMachineController : OwnBaseController
    {

        public ViewResult ListMerchant()
        {
            return View();
        }

        public ViewResult ListMachineByBinded()
        {
            return View();
        }

        public ViewResult ListMachineByBindable()
        {
            return View();
        }

        public CustomJsonResult GetMachineListByBindable(SearchCondition condition)
        {

            string deviceId = condition.DeviceId.ToSearchString();

            var list = (from u in CurrentDb.Machine
                        where u.IsUse == false &&
                               (deviceId.Length == 0 || u.DeviceId.Contains(deviceId))
                        select new { u.Id, u.Sn, u.Name, u.DeviceId, u.MacAddress, u.CreateTime });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }


        public CustomJsonResult GetMachineListByBinded(SearchCondition condition)
        {

            string deviceId = condition.DeviceId.ToSearchString();

            var list = (from mp in CurrentDb.MerchantMachine
                        join m in CurrentDb.Merchant on mp.MerchantId equals m.Id
                        join p in CurrentDb.Machine on mp.MachineId equals p.Id
                        where
                        p.IsUse == true &&
                        mp.Status == Enumeration.MerchantMachineStatus.Bind &&
                               (deviceId.Length == 0 || p.DeviceId.Contains(deviceId))

                        select new { mp.Id, mp.MerchantId, mp.MachineId, MerchantName = m.Name, p.DeviceId, MachineName = p.Name, p.MacAddress });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Bind(int merchantId, int machineId)
        {
            return BizFactory.MerchantMachine.Bind(this.CurrentUserId, merchantId, machineId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult UnBind(int merchantId, int machineId)
        {
            return BizFactory.MerchantMachine.UnBind(this.CurrentUserId, merchantId, machineId);
        }

    }
}