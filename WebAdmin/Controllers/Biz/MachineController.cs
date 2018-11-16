using System;
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

        public CustomJsonResult GetDetails(string machineId)
        {
            return AdminServiceFactory.Machine.GetDetails(this.CurrentUserId, machineId);
        }

        public CustomJsonResult GetList(RupMachineGetList rup)
        {
            string deviceId = rup.DeviceId.ToSearchString();

            var query = (from p in CurrentDb.Machine
                         where
                                 (deviceId.Length == 0 || p.DeviceId.Contains(deviceId))
                         select new { p.Id, p.Name, p.DeviceId, p.MacAddress, p.IsUse, p.CreateTime });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {

                string merchantId = "";
                string merchantName = "未绑定商户";
                bool isBind = false;
                var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MachineId == item.Id && m.IsBind == true).FirstOrDefault();
                if (merchantMachine != null)
                {
                    var merchant = CurrentDb.SysMerchantUser.Where(m => m.Id == merchantMachine.MerchantId).FirstOrDefault();
                    if (merchant != null)
                    {
                        merchantName = merchant.MerchantName;
                        isBind = true;
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
                    isBind,
                    item.MacAddress,
                    item.CreateTime
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
        public CustomJsonResult Bind(string merchantId, string machineId)
        {
            var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();
            if (machine.IsUse)
            {
                return AdminServiceFactory.Merchant.MachineBindOff(this.CurrentUserId, merchantId, machineId);
            }
            else
            {
                return AdminServiceFactory.Merchant.MachineBindOn(this.CurrentUserId, merchantId, machineId);
            }

        }
    }
}