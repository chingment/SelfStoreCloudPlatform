using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models
{
    public class InsPlanUtil
    {

        public static InsPlan Get1()
        {
            var insPlan = new InsPlan();

            insPlan.Cols.Add(new InsPlanCol { Id = 1, Title = "保障内容" });
            insPlan.Cols.Add(new InsPlanCol { Id = 2, Title = "方案一" });
            insPlan.Cols.Add(new InsPlanCol { Id = 3, Title = "方案二" });
            insPlan.Cols.Add(new InsPlanCol { Id = 4, Title = "方案三" });

            insPlan.Rows.Add(new InsPlanRow { Id = 1, Title = "被保险人年龄" });
            insPlan.Rows.Add(new InsPlanRow { Id = 2, Title = "意外身故/伤残" });
            insPlan.Rows.Add(new InsPlanRow { Id = 3, Title = "意外医疗" });
            insPlan.Rows.Add(new InsPlanRow { Id = 4, Title = "保费" });

            return insPlan;
        }
    }
}