using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class CalculateCarClaimPayPrice
    {
        private string _version = "2017.02.20";

        private decimal _commissionRate = 0.15m;

        private decimal _merchantCommission = 0;

        private decimal _merchantCommissionRate = 0.80m;

        private decimal _uplinkCommission = 0;

        private decimal _uplinkCommissionRate = 0.20m;

        private decimal _payPrice = 0;


        private decimal _totalCommission = 0;

        public decimal CommissionRate
        {
            get { return _commissionRate; }
        }

        public decimal MerchantCommissionRate
        {
            get { return _merchantCommissionRate; }
        }

        public decimal UplinkCommissionRate
        {
            get { return _uplinkCommissionRate; }
        }


        public decimal PayPrice
        {
            get { return _payPrice; }
        }


        public decimal MerchantCommission
        {
            get
            {
                return _merchantCommission;
            }
        }


        public decimal UplinkCommission
        {

            get
            {
                return _uplinkCommission;
            }
        }

        public decimal TotalCommission
        {

            get
            {
                return _totalCommission;
            }
        }


        public CalculateCarClaimPayPrice(decimal workingHoursPrice, decimal accessoriesPrice)
        {

            _payPrice = Math.Round((workingHoursPrice + accessoriesPrice) * _commissionRate, 2);

            _merchantCommission = Math.Round(_payPrice * _merchantCommissionRate, 2);

            _uplinkCommission = Math.Round(_payPrice * _uplinkCommissionRate, 2);

            _totalCommission = _merchantCommission + _uplinkCommission;
        }

        public string Version
        {
            get
            {
                return _version;
            }

        }
    }
}
