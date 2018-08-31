using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Order2StockIn
{
    public class AddViewModel : OwnBaseViewModel
    {
        public Lumos.Entity.Order2StockIn _order2StockIn = new Lumos.Entity.Order2StockIn();
        public List<Lumos.Entity.Order2StockInDetails> _order2StockInDetails = new List<Lumos.Entity.Order2StockInDetails>();
        public Lumos.Entity.Order2StockIn Order2StockIn
        {
            get
            {
                return _order2StockIn;
            }
            set
            {
                _order2StockIn = value;
            }
        }

        public List<Lumos.Entity.Order2StockInDetails> Order2StockInDetails
        {
            get
            {
                return _order2StockInDetails;
            }
            set
            {
                _order2StockInDetails = value;
            }
        }

        public AddViewModel()
        {

        }
    }
}