using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebBack.Models.Sys.LogView
{
    public class ListModel
    {
        public ListModel()
        {
            this.Files=new FileInfo[0];
            this.Dirs = new DirectoryInfo[0];
        }

        public String Parent {
            get;
            set;
        }
        public FileInfo[] Files{
            get;
            set;
        }
        public DirectoryInfo[] Dirs{
            get;
            set;
        }
    }
}