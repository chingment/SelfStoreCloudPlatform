using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public static class RedisMqFactory
    {
        public static RedisMq4GlobalProvider Global
        {
            get
            {
                return new RedisMq4GlobalProvider();
            }
        }
    }
}
