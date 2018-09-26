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
        private Lumos.Entity.SysMerchantUser _sysMerchantUser = new Lumos.Entity.SysMerchantUser();

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

        public Lumos.Entity.SysMerchantUser SysMerchantUser
        {
            get
            {
                return _sysMerchantUser;
            }
            set
            {
                _sysMerchantUser = value;
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
                var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MachineId == machine.Id && m.IsBind==true).FirstOrDefault();
                if (merchantMachine != null)
                {
                    var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == merchantMachine.MerchantId).FirstOrDefault();
                    if (sysMerchantUser != null)
                    {
                        _sysMerchantUser = sysMerchantUser;
                    }
                }
            }
        }
    }
}