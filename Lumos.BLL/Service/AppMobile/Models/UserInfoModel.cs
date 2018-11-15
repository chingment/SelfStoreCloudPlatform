using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class UserInfoModel
    {
        public string UserId { get; set; }

        public string NickName { get; set; }

        public string PhoneNumber { get; set; }

        public string HeadImgUrl { get; set; }

        public bool IsVip { get; set; }
    }
}
