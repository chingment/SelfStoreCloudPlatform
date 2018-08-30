using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SysUserProvider : BaseProvider
    {
        public string GetFullName(string pId)
        {
            if (pId == null)
                return "";

            string fullName = "";
            var user = CurrentDb.SysUser.Where(m => m.Id == pId).FirstOrDefault();
            if (user != null)
            {
                fullName = user.FullName;
            }

            return fullName;
        }
    }
}
