﻿using System;
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

        public ViewResult ListByBind()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        public ViewResult Edit(string id)
        {
            var model = new EditViewModel(id);
            return View(model);
        }

        public ViewResult Bind(string id)
        {
            var model = new BindViewModel(id);

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

            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {

                string merchantId = "";
                string merchantName = "未绑定商户";
                var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MachineId == item.Id && m.Status == Enumeration.MerchantMachineStatus.Bind).FirstOrDefault();
                if (merchantMachine != null)
                {
                    var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantMachine.MerchantId).FirstOrDefault();
                    if (merchant != null)
                    {
                        merchantName = merchant.Name;
                    }
                }

                olist.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    MerchantId = merchantId,
                    MerchantName = merchantName,
                    item.DeviceId,
                    item.IsUse,
                    UseStatusName = (item.IsUse == true ? "是" : "否"),
                    BindStatusName = (item.IsUse == true ? "已绑定" : "未绑定"),
                    item.MacAddress,
                    item.CreateTime
                });


            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Bind(string merchantId, string machineId)
        {
            var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();
            if (machine.IsUse)
            {
                return BizFactory.MerchantMachine.BindOff(this.CurrentUserId, merchantId, machineId);
            }
            else
            {
                return BizFactory.MerchantMachine.BindOn(this.CurrentUserId, merchantId, machineId);
            }
        }
    }
}