using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class RopLoginByMinProgram
    {
        public string Code { get; set; }

        public string Iv { get; set; }
        public string EncryptedData { get; set; }
    }
}
