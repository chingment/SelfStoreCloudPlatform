﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lumos.Common;
using Lumos.Entity;
using Lumos.Web.Mvc;
using Lumos.BLL;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Transactions;
using Lumos;
using Lumos.BLL.Service.Admin;

namespace WebAdmin.Controllers.Biz
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

        public ViewResult Edit()
        {
            return View();
        }

        public ViewResult Bind()
        {
            return View();
        }

        public CustomJsonResult GetDetails(string id)
        {
            return AdminServiceFactory.Machine.GetDetails(this.CurrentUserId, id);
        }

        public CustomJsonResult GetList(RupMachineGetList rup)
        {
            string deviceId = rup.DeviceId.ToSearchString();
            string merchantName = rup.MerchantName.ToSearchString();

            var query = (from p in CurrentDb.Machine
                         join a in CurrentDb.Merchant on p.MerchantId equals a.Id into temp
                         from tt in temp.DefaultIfEmpty()
                         where
                                 (merchantName.Length == 0 || tt.Name.Contains(merchantName)) &&
                                    (deviceId.Length == 0 || p.DeviceId.Contains(deviceId))
                         select new { p.Id, p.Name, p.DeviceId, p.MacAddress, p.CreateTime, p.MerchantId, MerchantName = tt.Name });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {
                bool l_isBindMerchant = string.IsNullOrEmpty(item.MerchantId) ? false : true;
                string l_merchantName = string.IsNullOrEmpty(item.MerchantId) ? "未绑定" : item.MerchantName;
                string l_statusName = "";
                olist.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    MerchantId = item.MerchantId,
                    MerchantName = l_merchantName,
                    IsBindMerchant = l_isBindMerchant,
                    DeviceId = item.DeviceId,
                    MacAddress = item.MacAddress,
                    StatusName = l_statusName,
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime()
                });


            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult Add(RopMachineAdd rop)
        {
            return AdminServiceFactory.Machine.Add(this.CurrentUserId, rop);
        }

        [HttpPost]

        public CustomJsonResult Edit(RopMachineEdit rop)
        {
            return AdminServiceFactory.Machine.Edit(this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Bind(string id, Enumeration.MachineBindType bindType, string merchantId)
        {
            if (bindType == Enumeration.MachineBindType.Off)
            {
                return AdminServiceFactory.Machine.BindOffMerchant(this.CurrentUserId, id, merchantId);
            }
            else
            {
                return AdminServiceFactory.Machine.BindOnMerchant(this.CurrentUserId, id, merchantId);
            }

        }
    }
}