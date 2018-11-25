using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class RetMachineGetDetails
    {
        private string _Id = "";
        private string _name = "";
        private string _deviceId = "";
        private string _macAddress = "";
        private bool _isBindMerchant = false;

        public RetMachineGetDetails()
        {
            this.Merchant = new MerchantModel();
        }

        public string Id
        {
            get
            {

                return _Id;
            }
            set
            {
                _Id = value;
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
        public bool IsBindMerchant
        {
            get
            {

                return _isBindMerchant;
            }
            set
            {
                _isBindMerchant = value;
            }
        }
        public MerchantModel Merchant { get; set; }

        public StoreModel Store { get; set; }

        public class MerchantModel
        {
            public string _id = "";
            private string _name = "";
            private string _contactName = "";
            private string _contactPhone = "";
            private string _contactAddress = "";

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

            public string ContactName
            {
                get
                {

                    return _contactName;
                }
                set
                {
                    _contactName = value;
                }
            }

            public string ContactPhone
            {
                get
                {

                    return _contactPhone;
                }
                set
                {
                    _contactPhone = value;
                }
            }

            public string ContactAddress
            {
                get
                {

                    return _contactAddress;
                }
                set
                {
                    _contactAddress = value;
                }
            }
        }

        public class StoreModel {


            public string Name { get; set; }

            public string Id { get; set; }
        }
    }
}
