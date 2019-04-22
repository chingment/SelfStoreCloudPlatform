using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class SpecModel
    {
        public SpecModel()
        {

            this.Value = new List<SpecValueModel>();
        }
           
          
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SpecValueModel> Value { get; set; }
    }
}
