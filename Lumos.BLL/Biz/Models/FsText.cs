using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{

    public class FsText
    {
        private string _Content = "";
        private string _Color = "";

        public FsText()
        {

        }

        public FsText(string content,string color)
        {
            _Content = content;
            _Color = color;
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
