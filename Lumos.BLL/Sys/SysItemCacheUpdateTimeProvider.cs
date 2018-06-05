using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SysItemCacheUpdateTimeProvider : BaseProvider
    {
        public void Update(Entity.Enumeration.SysItemCacheType type)
        {
            if (type != Entity.Enumeration.SysItemCacheType.User)
            {
                var sysItemCacheUpdateTime = CurrentDb.SysItemCacheUpdateTime.Where(m => m.Type == type).FirstOrDefault();
                if (sysItemCacheUpdateTime != null)
                {
                    sysItemCacheUpdateTime.LastUpdateTime = this.DateTime;
                    CurrentDb.SaveChanges();
                }
            }
        }

        public void UpdateUser(int userId)
        {
            var sysItemCacheUpdateTime = CurrentDb.SysItemCacheUpdateTime.Where(m => m.Type == Entity.Enumeration.SysItemCacheType.User && m.ReferenceId == userId).FirstOrDefault();
            if (sysItemCacheUpdateTime != null)
            {
                sysItemCacheUpdateTime.LastUpdateTime = this.DateTime;
                CurrentDb.SaveChanges();
            }
        }

        public bool CanGetData(int userId, DateTime? lastUpdateTime, out DateTime? updateTime)
        {
            updateTime = this.DateTime;
            return true;
        }

    }
}
