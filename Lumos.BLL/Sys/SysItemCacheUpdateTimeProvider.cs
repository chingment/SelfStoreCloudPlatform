using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SysItemCacheUpdateTimeProvider : BaseProvider
    {
        public void Update(Entity.Enumeration.SysItemCacheType pType)
        {
            if (pType != Entity.Enumeration.SysItemCacheType.User)
            {
                var sysItemCacheUpdateTime = CurrentDb.SysItemCacheUpdateTime.Where(m => m.Type == pType).FirstOrDefault();
                if (sysItemCacheUpdateTime != null)
                {
                    sysItemCacheUpdateTime.MendTime = this.DateTime;
                    CurrentDb.SaveChanges();
                }
            }
        }

        public void UpdateUser(string pUserId)
        {
            var sysItemCacheUpdateTime = CurrentDb.SysItemCacheUpdateTime.Where(m => m.Type == Entity.Enumeration.SysItemCacheType.User && m.ReferenceId == pUserId).FirstOrDefault();
            if (sysItemCacheUpdateTime != null)
            {
                sysItemCacheUpdateTime.MendTime = this.DateTime;
                CurrentDb.SaveChanges();
            }
        }

        public bool CanGetData(string pUserId, DateTime? pLastUpdateTime, out DateTime? pUpdateTime)
        {
            pUpdateTime = this.DateTime;
            return true;
        }

    }
}
