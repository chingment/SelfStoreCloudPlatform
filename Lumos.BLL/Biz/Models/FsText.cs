using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{

    public class FsText
    {
        private string _Title = "";
        private string _Content = "";
        private string _Color = "";

        public string Title {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }
        public string Content
        {
            get
            {
                return _Content;
            }
            set
            {
                _Content = value;
            }
        }
        public string Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
            }
        }
    }
}
