using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RetMachineGetDetails
    {
        private string _id = "";
        private string _name = "";
        private string _deviceId = "";
        private string _macAddress = "";
        private bool _isBindStore = false;
        private string _status = "";
        private string _statusName = "";
        public RetMachineGetDetails()
        {
            this.Skus = new List<SkuModel>();
            this.Store = new StoreModel();
        }

        public string Id
        {
            get
            {

                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public string Name
        {
            get
            {

                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string DeviceId
        {
            get
            {

                return _deviceId;
            }
            set
            {
                _deviceId = value;
            }
        }
        public string MacAddress
        {
            get
            {

                return _macAddress;
            }
            set
            {
                _macAddress = value;
            }
        }
        public bool IsBindStore
        {
            get
            {

                return _isBindStore;
            }
            set
            {
                _isBindStore = value;
            }
        }

        public string StatusName
        {
            get
            {

                return _statusName;
            }
            set
            {
                _statusName = value;
            }
        }

        public string Status
        {
            get
            {

                return _status;
            }
            set
            {
                _status = value;
            }
        }

        public List<SkuModel> Skus { get; set; }

        public StoreModel Store { get; set; }

        public class StoreModel {
            private string _id = "";
            private string _name = "";
            private string _address = "";
            public string Id
            {
                get
                {

                    return _id;
                }
                set
                {
                    _id = value;
                }
            }
            public string Name
            {
                get
                {

                    return _name;
                }
                set
                {
                    _name = value;
                }
            }
            public string Address
            {
                get
                {

                    return _address;
                }
                set
                {
                    _address = value;
                }
            }
        }

        public class SkuModel
        {
            public string Id { get; set; }

            public string SlotId { get; set; }

            public string Name { get; set; }

            public string ImgUrl { get; set; }

            public int Quantity { get; set; }

            public int LockQuantity { get; set; }

            public int SellQuantity { get; set; }

            public decimal SalePrice { get; set; }

            public decimal SalePriceByVip { get; set; }
        }
    }
}
