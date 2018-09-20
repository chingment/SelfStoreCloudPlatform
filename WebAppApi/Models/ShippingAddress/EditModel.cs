using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.ShippingAddress
{
    public class EditModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Receiver { get; set; }
        public string PhoneNumber { get; set; }
        public string Area { get; set; }
        public string AreaCode { get; set; }
        public string Address { get; set; }
        public bool IsDefault { get; set; }
    }
}