using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMobile.Models.Account
{
    public class LoginViewModel
    {

        public string Mobile { get; set; }
        public string VerifyCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

    }
}