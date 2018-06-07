using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Machine
{
    public class BindViewModel : OwnBaseViewModel
    {
        private Lumos.Entity.Machine _machine = new Lumos.Entity.Machine();
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        public Lumos.Entity.Machine Machine
        {
            get
            {
                return _machine;
            }
            set
            {
                _machine = value;
            }
        }

        public Lumos.Entity.Merchant Merchant
        {
            get
            {
                return _merchant;
            }
            set
            {
                _merchant = value;
            }
        }

        public BindViewModel()
        {

        }

        public BindViewModel(string id)
        {
            var machine = CurrentDb.Machine.Where(m => m.Id == id).FirstOrDefault();
            if (machine != null)
            {
                _machine = machine;
                var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MachineId == machine.Id && m.Status == Enumeration.MerchantMachineStatus.Bind).FirstOrDefault();
                if (merchantMachine != null)
                {
                    var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantMachine.MerchantId).FirstOrDefault();
                    if (merchant != null)
                    {
                        _merchant = merchant;
                    }
                }
            }
        }
    }
}