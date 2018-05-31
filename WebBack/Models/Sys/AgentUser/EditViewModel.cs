using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.AgentUser
{
    public class EditViewModel : OwnBaseViewModel
    {
        private SysAgentUser _sysAgentUser = new SysAgentUser();

        public SysAgentUser SysAgentUser
        {
            get
            {
                return _sysAgentUser;
            }
            set
            {
                _sysAgentUser = value;
            }
        }

        public EditViewModel()
        {

        }

        public EditViewModel(int id)
        {
            var sysAgentUser = CurrentDb.SysAgentUser.Where(m => m.Id == id).FirstOrDefault();
            if (sysAgentUser != null)
            {
                _sysAgentUser = sysAgentUser;
            }
        }
    }
}