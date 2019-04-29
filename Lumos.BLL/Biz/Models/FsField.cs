using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class FsField
    {
        public FsField()
        {
            this.Title = new FsText();
            this.Value = new FsText();
        }

        public FsField(string title, string titleColor,string value,string valueColor)
        {
            this.Title = new FsText();
            this.Value = new FsText();

            this.Title.Content = title;
            this.Title.Color = titleColor;
            this.Value.Content = value;
            this.Value.Color = valueColor;
        }

        public FsText Title
        {
            get; set;
        }

        public FsText Value
        {
            get; set;
        }
    }
}
