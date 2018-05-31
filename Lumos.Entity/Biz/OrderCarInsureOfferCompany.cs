using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class OrderCarInsureOfferCompany
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int InsuranceCompanyId { get; set; }

        public decimal? CommercialPrice { get; set; }

        public decimal? TravelTaxPrice { get; set; }

        public decimal? CompulsoryPrice { get; set; }


    }
}
