using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common
{
    public static class NPOIHelperUtil
    {
        public static string GetCellValue(ICell cell)
        {
            string value = null;
            try
            {
                if (cell == null)
                    return value;

                value = cell.ToString().Trim();

                if(CommonUtil.IsDateTime(value))
                {
                    value = cell.DateCellValue.ToUnifiedFormatDateTime();
                }

                //switch (cell.CellType)
                //{
                //    case CellType.String:
                //        value = cell.ToString().Trim();
                //        break;
                //    case CellType.Numeric:
                //        if (cell.DateCellValue != null)
                //        {
                //            value = cell.DateCellValue.ToUnifiedFormatDateTime();
                //        }
                //        else
                //        {
                //            value = cell.ToString().Trim();
                //        }
                //        break;
                //}

                return value;
            }
            catch(Exception ex)
            {
                return value;
            }
        }
    }
}
