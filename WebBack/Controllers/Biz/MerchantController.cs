using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lumos.Common;
using Lumos.BLL;
using WebBack.Models.Biz.Merchant;
using Lumos;

namespace WebBack.Controllers.Biz
{
    public class MerchantController : OwnBaseController
    {

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

        public ViewResult Edit(string id)
        {
            EditViewModel model = new EditViewModel(id);
            return View(model);
        }

        public ViewResult MachineListByBindable()
        {
            return View();
        }

        public CustomJsonResult GetList(SearchCondition condition)
        {

            string name = condition.Name.ToSearchString();
            var query = (from m in CurrentDb.SysMerchantUser
                         join u in CurrentDb.SysUser on m.Id equals u.Id
                         where
                                 (name.Length == 0 || m.MerchantName.Contains(name))
                         select new { m.Id, u.UserName, m.MerchantName, m.ContactName, m.ContactPhone, m.CreateTime });

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
                    item.UserName,
                    item.MerchantName,
                    item.ContactName,
                    item.ContactPhone,
                    item.CreateTime
                });


            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult Add(AddViewModel model)
        {
            model.SysMerchantUser.UserName = string.Format("{0}{1}", "M", model.SysMerchantUser.UserName);

            return BizFactory.Merchant.Add(this.CurrentUserId, model.SysMerchantUser, model.MerchantConfig);
        }

        [HttpPost]

        public CustomJsonResult Edit(EditViewModel model)
        {
            return BizFactory.Merchant.Edit(this.CurrentUserId, model.SysMerchantUser, model.MerchantConfig);
        }


        public CustomJsonResult GetMachineListByBinded(WebBack.Models.Biz.MerchantMachine.SearchCondition condition)
        {

            string deviceId = condition.DeviceId.ToSearchString();

            var list = (from mp in CurrentDb.MerchantMachine
                        join m in CurrentDb.SysMerchantUser on mp.MerchantId equals m.Id
                        join p in CurrentDb.Machine on mp.MachineId equals p.Id
                        where
                        p.IsUse == true &&
                        mp.IsBind == true &&
                               (deviceId.Length == 0 || p.DeviceId.Contains(deviceId))

                        select new { mp.Id, mp.MerchantId, mp.MachineId, m.MerchantName, p.DeviceId, MachineName = p.Name, p.MacAddress });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }


        public CustomJsonResult GetMachineListByBindable(WebBack.Models.Biz.MerchantMachine.SearchCondition condition)
        {

            string deviceId = condition.DeviceId.ToSearchString();

            var list = (from u in CurrentDb.Machine
                        where u.IsUse == false &&
                               (deviceId.Length == 0 || u.DeviceId.Contains(deviceId))
                        select new { u.Id, u.Name, u.DeviceId, u.MacAddress, u.CreateTime });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }

        [HttpPost]
        public CustomJsonResult BindOffMachine(string merchantId, string machineId)
        {
            return BizFactory.MerchantMachine.BindOff(this.CurrentUserId, merchantId, machineId);
        }


        [HttpPost]
        public CustomJsonResult BindOnMachine(string merchantId, string machineId)
        {
            return BizFactory.MerchantMachine.BindOn(this.CurrentUserId, merchantId, machineId);
        }
    }
}