using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class RetWxUserCheckedUser
    {
        public string OpenId { get; set; }
        public string UnionId { get; set; }
        public string AccessToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public string PhoneNumber { get; set; }
        public string HeadImgUrl { get; set; }
        public string Nickname { get; set; }
        public string Sex { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string ClientId { get; set; }
    }
}
