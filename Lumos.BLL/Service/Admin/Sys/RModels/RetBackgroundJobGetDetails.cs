using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class RetBackgroundJobGetDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public string JobArgs { get; set; }
        public string CronExpression { get; set; }
        public string CronExpressionDescription { get; set; }
    }
}
