using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class RecipientMode
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class RecipientModeProvider
    {
        public List<RecipientMode> GetList()
        {
            var list = new List<RecipientMode>();


            list.Add(new RecipientMode { Id = "1", Name = "机器自取" });
            list.Add(new RecipientMode { Id = "2", Name = "快递寄送" });

            return list;
        }
    }
}
