using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class StoreService : BaseProvider
    {
        public List<StoreModel> List(string pOperater, string pClientId, RupStoreList rup)
        {

            var stores = CurrentDb.Store.Where(m => m.Status == Entity.Enumeration.StoreStatus.Opened).ToList();

            if (!string.IsNullOrEmpty(rup.MerchantId))
            {
                stores = stores.Where(m => m.MerchantId == rup.MerchantId).ToList();
            }

            var storeModels = new List<StoreModel>();
            foreach (var m in stores)
            {
                double distance = 0;
                string distanceMsg = "";

                distance = DistanceUtil.GetDistance(m.Lat, m.Lng, rup.Lat, rup.Lng);

                distanceMsg = string.Format("{0}km", distance.ToString("f2"));

                storeModels.Add(new StoreModel { Id = m.Id, Name = m.Name, Address = m.Address, DistanceMsg = distanceMsg });
            }

            return storeModels;
        }
    }
}
