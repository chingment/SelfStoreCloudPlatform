using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            text(DateTime.Parse("2012-1-2 15:02"), DateTime.Parse("2012-1-3 3:03"));
        }

        public static void text(DateTime starttime, DateTime endtime)
        {
            int days = (int)Math.Ceiling((endtime - starttime).TotalDays);

            for (var i = 0; i < days; i++)
            {
                DateTime time1 = DateTime.Parse(starttime.AddDays(i).ToString("yyyy-MM-dd") + " " + " 0:00");
                DateTime time2 = DateTime.Parse(starttime.AddDays(i).ToString("yyyy-MM-dd") + " " + " 2:00");
                DateTime time3 = DateTime.Parse(starttime.AddDays(i).ToString("yyyy-MM-dd") + " " + " 8:00");
                DateTime time4 = DateTime.Parse(starttime.AddDays(i).ToString("yyyy-MM-dd") + " " + " 23:59");


                if (starttime >= time1 && starttime <= time2)
                {

                }
                else if (starttime >= time2 && starttime <= time3)
                {

                }
                else if (starttime >= time3 && starttime <= time4)
                {
                    DateTime nextTime1 = DateTime.Parse(starttime.AddDays(1).ToString("yyyy-MM-dd") + " " + " 2:00");
                    DateTime nextTime2 = DateTime.Parse(starttime.AddDays(1).ToString("yyyy-MM-dd") + " " + " 8:00");

                    if (endtime < nextTime1)
                    {
                        double totalstMinutes = (endtime - starttime).TotalMinutes;
                        decimal sTBeforePerHourPrice = 30;
                        double sTBeforeMinutePerPrice = double.Parse(sTBeforePerHourPrice.ToString()) / 60;
                        int sTBeforeHour = 3;
                        double sTBeforeMinute = sTBeforeHour * 60;

                        double sTFee = 0;

                        decimal sTPerHourPrice = 25;

                        if (totalstMinutes <= sTBeforeMinute)
                        {
                            sTFee = totalstMinutes * sTBeforeMinutePerPrice;
                        }
                        else
                        {

                            double sTFee1 = sTBeforeMinute * sTBeforeMinutePerPrice;

                            double sTMinutes = totalstMinutes - sTBeforeMinute;

                            double sTPerMinutePrice = double.Parse(sTPerHourPrice.ToString()) / 60;

                            double sTFee2 = sTMinutes * sTBeforeMinutePerPrice;

                            sTFee = sTFee1 + sTFee2;

                        }
                    }
                    else
                    {
                        //double sTFee = 0;

                        //if (endTime >= time1begin)
                        //{
                        //    double totalstMinutes = (endTime - time1begin).TotalMinutes;

                        //    double aNPerMinutePrice = double.Parse(aNPerHourPrice.ToString()) / 60;

                        //    sTFee = totalstMinutes * aNPerMinutePrice;

                        //}

                    }

                    //if (endtime < time2)
                    //{

                    //}
                    //else if (endtime < time3)
                    //{

                    //}
                    //else if (endtime > time3)
                    //{

                    //}

                }

                //if (starttime > time1end)
                //{
                //    time1begin = time1begin.AddDays(1);
                //    time1end = time1end.AddDays(1);
                //}



            }



        }
    }
}
