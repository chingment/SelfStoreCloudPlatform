using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Order
{
    public class DetailsViewModel : OwnBaseViewModel
    {

        private Lumos.Entity.Order _order = new Lumos.Entity.Order();
        private List<Lumos.Entity.OrderDetails> _orderDetails = new List<Lumos.Entity.OrderDetails>();
        private List<Lumos.Entity.OrderDetailsChild> _orderDetailsChild = new List<Lumos.Entity.OrderDetailsChild>();
        private List<Lumos.Entity.OrderDetailsChildSon> _orderDetailsChildSon = new List<Lumos.Entity.OrderDetailsChildSon>();


        public Lumos.Entity.Order Order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
            }
        }

        public List<Lumos.Entity.OrderDetails> OrderDetails
        {
            get
            {
                return _orderDetails;
            }
            set
            {
                _orderDetails = value;
            }
        }

        public List<Lumos.Entity.OrderDetailsChild> OrderDetailsChild
        {
            get
            {
                return _orderDetailsChild;
            }
            set
            {
                _orderDetailsChild = value;
            }
        }

        public List<Lumos.Entity.OrderDetailsChildSon> OrderDetailsChildSon
        {
            get
            {
                return _orderDetailsChildSon;
            }
            set
            {
                _orderDetailsChildSon = value;
            }
        }

        public void LoadData(string id)
        {
            var order = CurrentDb.Order.Where(m => m.Id == id).FirstOrDefault();
            if (order != null)
            {
                _order = order;

                var orderDetails = CurrentDb.OrderDetails.Where(m => m.OrderId == id).ToList();

                if (orderDetails != null)
                {
                    _orderDetails = orderDetails;


                }

                var orderDetailsChild = CurrentDb.OrderDetailsChild.Where(m => m.OrderId == id).ToList();

                if (orderDetailsChild != null)
                {
                    _orderDetailsChild = orderDetailsChild;


                }

                var orderDetailsChildSon = CurrentDb.OrderDetailsChildSon.Where(m => m.OrderId == id).ToList();

                if (orderDetailsChildSon != null)
                {
                    _orderDetailsChildSon = orderDetailsChildSon;

                }
            }
        }


    }
}