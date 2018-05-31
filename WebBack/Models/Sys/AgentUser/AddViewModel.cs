using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.AgentUser
{
    public class AddViewModel:OwnBaseViewModel
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
    }
}