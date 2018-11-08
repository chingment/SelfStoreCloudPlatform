using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.WebBack
{
    public class RetSysStaffUserGetInitDataByAddView
    {
        [JsonConverter(typeof(JsonObjectConvert))]
        public string Roles { get; set; }
    }
}
