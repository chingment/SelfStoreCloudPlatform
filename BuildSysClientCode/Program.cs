
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BuildSysClientCode
{
    class Program
    {
        [DllImport("User32.dll", EntryPoint = "ShowWindow")]
        private static extern bool ShowWindow(IntPtr hWnd, int type);

        static void Main(string[] args)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; ;
            int start = 1;
            int end = 999999;

            List<int> list = new List<int>();
            for (var i = start; i <= end; i++)
            {
                list.Add(i);
            }

            List<int> myList = ListRandom(list);

            for (var i = 1; i < myList.Count; i++)
            {

                string code = myList[i].ToString().PadLeft(6, '0');
                string sql = "insert into SysClientCode values('" + i + "','" + code + "')";
                SqlHelper.ExecuteNonQuery(connectionString, System.Data.CommandType.Text, sql, null);

            }
            Console.WriteLine("完成");
            Console.ReadLine();

        }

        private static List<int> ListRandom(List<int> myList)
        {

            Random ran = new Random();
            List<int> newList = new List<int>();
            int index = 0;
            int temp = 0;
            for (int i = 0; i < myList.Count; i++)
            {

                index = ran.Next(0, myList.Count - 1);
                if (index != i)
                {
                    temp = myList[i];
                    myList[i] = myList[index];
                    myList[index] = temp;
                }
            }
            return myList;
        }
    }
}
