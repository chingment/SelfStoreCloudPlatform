using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models.Machine
{
    public class EditViewModel : OwnBaseViewModel
    {
        private Lumos.Entity.Machine _machine = new Lumos.Entity.Machine();


        public EditViewModel()
        {

        }

        public EditViewModel(int id)
        {
            var posMachine = CurrentDb.Machine.Where(m => m.Id == id).FirstOrDefault();
            if (posMachine != null)
            {
                _machine = posMachine;
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