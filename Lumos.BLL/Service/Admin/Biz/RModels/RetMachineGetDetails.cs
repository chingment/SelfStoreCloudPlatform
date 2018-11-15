using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class RetMachineGetDetails
    {
        private string _machineId = "";
        private string _name = "";
        private string _deviceId = "";
        private string _macAddress = "";
        private bool _isUse = false;

        public RetMachineGetDetails()
        {
            this.Merchant = new MerchantModel();
        }

        public string MachineId
        {
            get
            {

                return _machineId;
            }
            set
            {
                _machineId = value;
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
        public bool IsUse
        {
            get
            {

                return _isUse;
            }
            set
            {
                _isUse = value;
            }
        }
        public MerchantModel Merchant { get; set; }

        public StoreModel Store { get; set; }

        public class MerchantModel
        {
            public string _merchantId = "";
            private string _name = "";
            private string _contactName = "";
            private string _contactPhone = "";
            private string _contactAddress = "";

            public string MerchantId
            {
                get
                {

                    return _merchantId;
                }
                set
                {
                    _merchantId = value;
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

            public string StoreId { get; set; }
        }
    }
}
