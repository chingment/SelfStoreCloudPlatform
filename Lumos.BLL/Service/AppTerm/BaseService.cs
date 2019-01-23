using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public class TicketInfo
    {
        public string UserId { get; set; }
        public string MerchantId { get; set; }

        public string StoreId { get; set; }

        public string MachineId { get; set; }
    }


    public class BaseService : BaseProvider
    {

        public TicketInfo GetTicketInfo(string ticket)
        {
            var ticketInfo = new TicketInfo();
            var storeMachine = CurrentDb.StoreMachine.Where(m => m.Id == ticket && m.IsBind == true).FirstOrDefault();
            if (storeMachine != null)
            {
                ticketInfo.UserId = storeMachine.UserId;
                ticketInfo.MerchantId = storeMachine.MerchantId;
                ticketInfo.StoreId = storeMachine.StoreId;
                ticketInfo.MachineId = storeMachine.MachineId;
            }
            return ticketInfo;
        }
    }
}
