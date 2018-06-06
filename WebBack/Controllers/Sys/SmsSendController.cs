using Lumos;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Sys.SmsSend;
namespace WebBack.Controllers.Sys
{
    public class SmsSendController : OwnBaseController
    {
        // GET: SmsSend
        public ActionResult Logs()
        {
            return View();
        }

        public CustomJsonResult GetLogs(SearchCondition condition)
        {
            var query = (from u in CurrentDb.SysSmsSendHistory
                         where (condition.Phone == null || u.Phone.Contains(condition.Phone)) &&
                         u.ValidCode != null
                         select new { u.Id, u.Phone, u.ValidCode, u.ExpireTime, u.Result, u.FailureReason, u.CreateTime });

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
                    item.Phone,
                    item.ValidCode,
                    item.ExpireTime,
                    item.CreateTime,
                    item.FailureReason,
                    Result = item.Result.GetCnName()
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }
    }
}