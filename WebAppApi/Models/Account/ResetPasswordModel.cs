using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Account
{
    public class ResetPasswordModel
    {
        public string UserName { get; set; }

        public string Token { get; set; }

        public string ValidCode { get; set; }

        public string Password { get; set; }
    }
}