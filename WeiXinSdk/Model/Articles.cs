using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class Article
    {
        public Article()
        {
            this.author = new Author();
        }

        public string thumb_media_id { get; set; }

        public Author author { get; set; }

        public string show_cover_pic { get; set; }

    }
}
