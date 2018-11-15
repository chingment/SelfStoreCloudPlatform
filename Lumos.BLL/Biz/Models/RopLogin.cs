using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class RopLogin
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsRememberMe { get; set; }

        public string VerifyCode { get; set; }

        public string ReturnUrl { get; set; }
    }
}
