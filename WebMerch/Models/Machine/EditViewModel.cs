using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Machine
{
    public class EditViewModel : OwnBaseViewModel
    {
        private Lumos.Entity.MerchantMachine _merchantMachine = new Lumos.Entity.MerchantMachine();
        private Lumos.Entity.Machine _machine = new Lumos.Entity.Machine();
        private Lumos.Entity.Store _store = new Lumos.Entity.Store();

        public EditViewModel()
        {

        }

        public void LoadData(string id)
        {
            var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.Id == id && m.UserId == this.Operater).FirstOrDefault();
            if (merchantMachine != null)
            {
                _merchantMachine = merchantMachine;

                var machine = CurrentDb.Machine.Where(m => m.Id == merchantMachine.MachineId).FirstOrDefault();
                if (machine != null)
                {
                    _machine = machine;
                }

                var storeMachine = CurrentDb.StoreMachine.Where(m => m.MachineId == merchantMachine.MachineId && m.Status == Enumeration.StoreMachineStatus.Bind).FirstOrDefault();
                if (storeMachine != null)
                {
                    var store = CurrentDb.Store.Where(m => m.Id == storeMachine.StoreId).FirstOrDefault();
                    if (store != null)
                    {
                        _store = store;
                    }
                }
            }
        }

        public Lumos.Entity.MerchantMachine MerchantMachine
        {
            get
            {
                return _merchantMachine;
            }
            set
            {
                _merchantMachine = value;
            }
        }

        public Lumos.Entity.Store Store
        {
            get
            {
                return _store;
            }
            set
            {
                _store = value;
            }
        }

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
    }
}