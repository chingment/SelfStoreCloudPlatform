﻿using Lumos.Entity;
using Lumos.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lumos.Common;
using Lumos;
using Lumos.BLL.Service.Admin;

namespace WebAdmin.Controllers.Biz
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

        public ViewResult Edit()
        {
            return View();
        }

        public ViewResult MachineListByBindable()
        {
            return View();
        }

        public CustomJsonResult GetDetails(string merchantId)
        {
            return AdminServiceFactory.Merchant.GetDetails(this.CurrentUserId, merchantId);
        }

        public CustomJsonResult GetList(RupMachineGetList rup)
        {
            string name = rup.Name.ToSearchString();
            var query = (from m in CurrentDb.SysMerchantUser
                         join u in CurrentDb.SysUser on m.Id equals u.Id
                         where
                                 (name.Length == 0 || m.MerchantName.Contains(name))
                         select new { m.Id, u.UserName, m.MerchantName, m.ContactName, m.ContactAddress, m.ContactPhone, m.CreateTime });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
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
                    item.CreateTime,
                    item.ContactAddress
                });


            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult Add(RopMerchantAdd rop)
        {
            return AdminServiceFactory.Merchant.Add(this.CurrentUserId, rop);
        }

        [HttpPost]

        public CustomJsonResult Edit(RopMerchantEdit rop)
        {
            return AdminServiceFactory.Merchant.Edit(this.CurrentUserId, rop);
        }


        public CustomJsonResult GetMachineListByBinded(RupMachineGetList rup)
        {

            string deviceId = rup.DeviceId.ToSearchString();

            var list = (from mp in CurrentDb.MerchantMachine
                        join m in CurrentDb.SysMerchantUser on mp.MerchantId equals m.Id
                        join p in CurrentDb.Machine on mp.MachineId equals p.Id
                        where
                        mp.MerchantId == rup.MerchantId &&
                        p.IsUse == true &&
                        mp.IsBind == true &&
                               (deviceId.Length == 0 || p.DeviceId.Contains(deviceId))

                        select new { mp.Id, mp.MerchantId, mp.MachineId, m.MerchantName, p.DeviceId, MachineName = p.Name, p.MacAddress });

            int total = list.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }


        public CustomJsonResult GetMachineListByBindable(RupMachineGetList rup)
        {

            string deviceId = rup.DeviceId.ToSearchString();

            var list = (from u in CurrentDb.Machine
                        where u.IsUse == false &&
                               (deviceId.Length == 0 || u.DeviceId.Contains(deviceId))
                        select new { u.Id, u.Name, u.DeviceId, u.MacAddress, u.CreateTime });

            int total = list.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }

        [HttpPost]
        public CustomJsonResult BindOffMachine(string merchantId, string machineId)
        {
            return AdminServiceFactory.Merchant.BindOffMachine(this.CurrentUserId, merchantId, machineId);
        }


        [HttpPost]
        public CustomJsonResult BindOnMachine(string merchantId, string machineId)
        {
            return AdminServiceFactory.Merchant.BindOnMachine(this.CurrentUserId, merchantId, machineId);
        }
    }
}