using System.Web.Mvc;
using Lumos.BLL;
using Lumos;
using Lumos.BLL.Sys;

namespace WebMerch.Controllers
{
    public class HomeController : OwnBaseController
    {

        public ViewResult Index()
        {
            BizFactory.ProductSku.InitSearchCache();

           

            //var tran = RedisManager.Db.CreateTransaction();

            //var productSkus = CurrentDb.ProductSku.Where(m => m.BarCode != null).ToList();

            //foreach (var item in productSkus)
            //{

            //    tran.HashSetAsync("h_productSkus", "barcode:" + item.BarCode + ",name:" + item.Name, Newtonsoft.Json.JsonConvert.SerializeObject(item));
            //    tran.SortedSetIncrementAsync("productSkus", item.BarCode, 1);
            //}
            //tran.ExecuteAsync();

            //string key = "85";
            //string key2 = "绿力";
            //var res = RedisManager.Db.Sort("productSkus", 0, -1);
            //var res2 = RedisManager.Db.HashGetAll("h_productSkus");



            //var list = (from i in res select i.ToString()).Where(x => x.Contains(key)).Take(10).ToList();

            //var list3 = (from i in res2 select i).Where(x => x.Name.ToString().Contains(key)).Take(10).ToList();
            //var list4 = (from i in res2 select i).Where(x => x.Name.ToString().Contains(key2)).Take(10).ToList();

            //var res3 = RedisManager.Db.HashValues("h_productSkus");

            //List<ProductSku> p = new List<ProductSku>();
            //if (res3 != null && res3.Length > 0)
            //{
            //    foreach (var item in res3)
            //    {
            //        var value = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductSku>(item);
            //        p.Add(value);
            //    }
            //}

            //var list2 = (from i in p select i).Where(x => x.BarCode.Contains(key)).Take(10).ToList();

            return View();
        }

        public ViewResult Main()
        {
            return View();
        }

        public ViewResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// 退出方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogOff()
        {
            OwnRequest.Quit();

            return Redirect(OwnWebSettingUtils.GetLoginPage(""));
        }

        [HttpPost]
        public CustomJsonResult ChangePassword(RopChangePassword rop)
        {
            SysFactory.AuthorizeRelay.ChangePassword(this.CurrentUserId, this.CurrentUserId, rop.OldPassword, rop.NewPassword);

            return Json(ResultType.Success, "点击<a href=\"" + OwnWebSettingUtils.GetLoginPage("") + "\">登录</a>");

        }

    }
}