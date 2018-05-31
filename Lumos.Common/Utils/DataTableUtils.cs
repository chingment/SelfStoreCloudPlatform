using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common
{
    public static class DataTableUtils
    {
        public static DataTable ToStringDataTable(this DataTable argDataTable)
        {
            DataTable dtResult = new DataTable();
            //克隆表结构
            dtResult = argDataTable.Clone();
            foreach (DataColumn col in dtResult.Columns)
            {
                col.DataType = typeof(String);
            }
            for (int j = 0; j < argDataTable.Rows.Count; j++)
            {
                DataRow rowNew = dtResult.NewRow();
                for (int i = 0; i < argDataTable.Columns.Count; i++)
                {
                    rowNew[i] = argDataTable.Rows[j][i].ToString();
                }
                dtResult.Rows.Add(rowNew);
            }
            //返回希望的结果
            return dtResult;
        }

        public static DataTable CopyToDataTable<T>(this IEnumerable<T> array)
        {
            var ret = new DataTable();
            foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                ret.Columns.Add(dp.Name, dp.PropertyType);
            foreach (T item in array)
            {
                var Row = ret.NewRow();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                    Row[dp.Name] = dp.GetValue(item);
                ret.Rows.Add(Row);
            }
            return ret;
        }

    }
}
