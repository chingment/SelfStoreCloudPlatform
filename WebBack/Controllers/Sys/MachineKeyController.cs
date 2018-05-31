using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBack.Controllers.Sys
{
    public class MachineKeyController : Controller
    {


        protected string CreateKey(int len)
        {

            byte[] bytes = new byte[len];

            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(bytes);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {

                sb.Append(string.Format("{0:X2}", bytes[i]));

            }

            return sb.ToString();

        }

        public ActionResult Index()
        {
            ViewBag.ValidationKey = CreateKey(20);
            ViewBag.DecryptionKey = CreateKey(24);
            return View();
        }
	}
}