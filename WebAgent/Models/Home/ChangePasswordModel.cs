using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.Home
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}