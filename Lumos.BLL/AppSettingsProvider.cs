using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class AppSettingsProvider
    {
        public int CutOffIntervalByMinutes
        {
            get
            {
                string blacklistPath = ConfigurationManager.AppSettings["custom:CutOffIntervalByMinutes"];

                if (blacklistPath == null)
                    return 60;

                return int.Parse(blacklistPath);
            }
        }

    }
}
