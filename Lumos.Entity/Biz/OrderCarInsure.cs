using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("OrderCarInsure")]
    public class OrderCarInsure : Order
    {
        public int InsuranceCompanyId { get; set; }

        public string CZCLXSImg { get; set; }

        public string CZSFZImg { get; set; }

        public string CCSJFWSZMImg { get; set; }

        public string YCZCLDJImg { get; set; }

        public decimal CommercialPrice { get; set; }

        public decimal TravelTaxPrice { get; set; }

        public decimal CompulsoryPrice { get; set; }

        public DateTime? StartOfferTime { get; set; }

        public DateTime? EndOfferTime { get; set; }



    }
}
