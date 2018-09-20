using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class UserInfoModel
    {
        public string UserId { get; set; }

        public string NickName { get; set; }

        public string Phone { get; set; }

        public string HeadImg { get; set; }

        public bool IsVip { get; set; }
    }
}
