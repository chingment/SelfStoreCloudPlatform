using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.Entity;
namespace Lumos.BLL.Service.Merch
{
    public class RetUserGetDetails
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OrganizationId { get; set; }
        public Enumeration.SysPositionId PositionId { get; set; }
        public Enumeration.UserStatus Status { get; set; }
        public string TeleSeatId { get; set; }
    }
}
