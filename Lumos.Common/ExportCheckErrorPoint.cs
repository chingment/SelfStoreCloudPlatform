using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common
{

    public class ExportErrorPoint
    {
        public ExportErrorPoint()
        {
            this.Point = new List<string>();
        }

        public string Key { get; set; }

        public List<string> Point { get; set; }

    }

    public class ExportCheckErrorPoint
    {

        public bool TitleHasError
        {
            get
            {
                if (_titleError.Contains(false))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        private List<bool> _titleError = new List<bool>();

        public bool CheckCellTitle(object cell, string title)
        {
            bool flag = true;
            if (cell == null)
            {
                flag = false;
                return flag;
            }

            string l_title = cell.ToString().Trim();
            if (l_title != title)
            {
                flag = false;


            }

            _titleError.Add(flag);

            return flag;
        }

        public List<ExportErrorPoint> ErrorPoint { get; set; }

        public ExportCheckErrorPoint()
        {
            this.ErrorPoint = new List<ExportErrorPoint>();
        }


        public void AddPoint(string key, string point)
        {
            var u = this.ErrorPoint.Where(c => c.Key == key).FirstOrDefault();
            if (u == null)
            {
                u = new ExportErrorPoint();
                u.Key = key;
                u.Point.Add(point);
                this.ErrorPoint.Add(u);
            }
            else
            {
                if (!u.Point.Contains(point))
                {
                    u.Point.Add(point);
                }
            }
        }
    }
}
