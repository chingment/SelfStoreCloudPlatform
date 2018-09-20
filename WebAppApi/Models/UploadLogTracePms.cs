using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models
{
    public class UploadLogTracePms
    {
        public string Trace { get; set; }
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        public int PosMachineId { get; set; }
    }
}