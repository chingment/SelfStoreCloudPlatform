﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class RopUserDeliveryAddressEdit
    {
        public string Id { get; set; }
        public string Consignee { get; set; }
        public string PhoneNumber { get; set; }
        public string AreaName { get; set; }
        public string AreaCode { get; set; }
        public string Address { get; set; }
        public bool IsDefault { get; set; }
    }
}