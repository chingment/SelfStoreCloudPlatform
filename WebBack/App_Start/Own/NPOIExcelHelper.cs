using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.POIFS;
using NPOI.Util;
using System.Text.RegularExpressions;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace WebBack
{
    public class NPOIExcelHelper
    {
        // <summary>  
        /// 将DataTable导出到Excel  
        /// </summary>  
        /// <param name="htmlTable">html表格内容</param>   
        /// <param name="fileName">仅文件名（非路径）</param>   
        /// <returns>返回Excel文件绝对路径</returns>  
        public static string ExportHtmlTableToExcel(string htmlTable, string fileName)
        {
            string result = "";
            try
            {
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                ISheet hssfSheet = hssfworkbook.CreateSheet(fileName);//创意一个Sheet表格
                int readRow = 0, readCol = 0;//正在读取的行,正在读取的列
                int totalRow = 0, totalCol = 0;//总行数,总列数

                htmlTable = htmlTable.Replace("\"", "'");//过滤单引号,用双引号替换
                var trReg = new Regex(pattern: @"(?<=(<[t|T][r|R]))[\s\S]*?(?=(</[t|T][r|R]>))");//获取表格行的正则表达
                var trMatchCollection = trReg.Matches(htmlTable);
                totalRow = trMatchCollection.Count;//获取总行数
                if (totalRow > 0)
                {
                    //获取第一行的属性集合
                    var rowTr = "<tr " + trMatchCollection[0].ToString().Trim() + "</tr>";
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    MatchCollection mcAttr = Regex.Matches(rowTr, @"([^\s=]+)=(['""\s]?)([^'""]+)\2(?=\s|$|>)");//获取属性集合
                    foreach (Match m in mcAttr)
                    {
                        if (m.Groups[1].Value.ToLower() == "colspan")
                        {
                            totalCol += int.Parse(m.Groups[3].Value);
                        }
                    }

                    //构建二维表数据结构,默认每个表格的值为false,代表没有读取
                    object[][] excelTable = new object[totalRow][];
                    for (int i = 0; i < totalRow; i++)
                    {
                        excelTable[i] = new object[totalCol];
                        for (int t = 0; t < totalCol; t++)
                        {
                            excelTable[i][t] = false;
                        }
                    }

                    //循环每行
                    for (int z = 0; z < totalRow; z++)
                    {
                        readRow = z;//正在读取的行
                        readCol = 0;//正在读取的列
                        var row = "<tr " + trMatchCollection[z].ToString().Trim() + "</tr>";
                        var tdReg = new Regex(pattern: @"(?<=(<[t|T][d|D|h|H]))[\s\S]*?(?=(</[t|T][d|D|h|H]>))");
                        var tdMatchCollection = tdReg.Matches(row);//获取行的列集合
                        for (int c = 0; c < tdMatchCollection.Count; c++)
                        {
                            int CellColSpan = 1;//列的合并列数
                            int CellRowSpan = 1;//列的合并行数
                            MatchCollection mc = Regex.Matches(tdMatchCollection[c].ToString(), @"([^\s=]+)=(['""\s]?)([^'""]+)\2(?=\s|$|>)");//列数据集合
                            foreach (Match m in mc)
                            {
                                if (m.Groups[1].Value.ToLower() == "colspan")
                                {
                                    CellColSpan = int.Parse(m.Groups[3].Value);//赋值列的合并行数
                                }
                                else if (m.Groups[1].Value.ToLower() == "rowspan")
                                {
                                    CellRowSpan = int.Parse(m.Groups[3].Value);//赋值列的合并行数
                                }
                            }

                            //正在读取的行列
                            for (int y = 0; y < totalCol; y++)
                            {
                                if (excelTable[readRow][y].ToString().ToLower() == "false")
                                {
                                    readCol = y;
                                    break;
                                }
                            }

                            //判断行列是否有合并
                            if (CellColSpan * CellRowSpan > 1)
                            {
                                IRow hssfrow = hssfSheet.CreateRow(readRow);//根据索引创建行
                                var cellRangeAddress = new CellRangeAddress(readRow, readRow + CellRowSpan - 1, readCol, readCol + CellColSpan - 1);
                                hssfSheet.AddMergedRegion(cellRangeAddress);//合并行列

                                var tdValue = RemoveHtml("<td " + tdMatchCollection[c].ToString().Trim() + "</td>");//过滤html格式获取td内容
                                hssfrow.CreateCell(readCol).SetCellValue(tdValue);//设置表格内容

                                //赋值表格已经读取
                                for (int frow = readRow; frow <= CellRowSpan + readRow - 1; frow++)
                                {
                                    for (int fcol = readCol; fcol <= CellColSpan + readCol - 1; fcol++)
                                    {
                                        excelTable[frow][fcol] = true;
                                    }
                                }
                                readCol = readCol + CellColSpan;
                            }
                            else
                            {

                                IRow hssfrow = hssfSheet.CreateRow(readRow);//根据索引创建行
                                var cellRangeAddress = new CellRangeAddress(readRow, readRow, readCol, readCol);
                                hssfSheet.AddMergedRegion(cellRangeAddress);//合并行列
                                var tdValue = RemoveHtml("<td " + tdMatchCollection[c].ToString().Trim() + "</td>");//过滤html格式获取td内容
                                hssfrow.CreateCell(readCol).SetCellValue(tdValue);//设置表格内容
                                //赋值表格已经读取
                                excelTable[readRow][readCol] = true;
                            }
                        }
                    }


                    FileStream file = new FileStream(HttpContext.Current.Request.PhysicalApplicationPath + "Temp/" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xls", FileMode.Create);
                    hssfworkbook.Write(file);
                    file.Close();
                }

            }
            catch (Exception ex)
            {
                result = "err_" + ex.Message;
            }
            return result;
        }


        public static void HtmlTable2Excel(string htmlTable, string sheetName)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet hssfSheet = hssfworkbook.CreateSheet(sheetName);//创意一个Sheet表格
            int readRow = 0, readCol = 0;//正在读取的行,正在读取的列
            int totalRow = 0, totalCol = 0;//总行数,总列数

            htmlTable = htmlTable.Replace("\"", "'");//过滤单引号,用双引号替换
            var trReg = new Regex(pattern: @"(?<=(<[t|T][r|R]))[\s\S]*?(?=(</[t|T][r|R]>))");//获取表格行的正则表达
            var trMatchCollection = trReg.Matches(htmlTable);
            totalRow = trMatchCollection.Count;//获取总行数
            if (totalRow > 0)
            {
                //获取第一行的属性集合
                var rowTr = "<tr " + trMatchCollection[0].ToString().Trim() + "</tr>";
                Dictionary<string, string> dic = new Dictionary<string, string>();
                MatchCollection mcAttr = Regex.Matches(rowTr, @"([^\s=]+)=(['""\s]?)([^'""]+)\2(?=\s|$|>)");//获取属性集合

                if (mcAttr.Count == 0)
                {
                    MatchCollection mcAttrCol = Regex.Matches(rowTr, @"(?<=(<[t|T][h|H]))[\s\S]*?(?=(</[t|T][h|H]>))");//获取属性集合
                    totalCol = mcAttrCol.Count;
                }
                else
                {
                    foreach (Match m in mcAttr)
                    {
                        if (m.Groups[1].Value.ToLower() == "colspan")
                        {
                            totalCol += int.Parse(m.Groups[3].Value);
                        }
                        else
                        {
                            totalCol += 1;
                        }
                    }
                }

                //构建二维表数据结构,默认每个表格的值为false,代表没有读取
                object[][] excelTable = new object[totalRow][];
                for (int i = 0; i < totalRow; i++)
                {
                    excelTable[i] = new object[totalCol];

                    for (int t = 0; t < totalCol; t++)
                    {
                        excelTable[i][t] = false;
                    }
                }

                //循环每行
                for (int z = 0; z < totalRow; z++)
                {
                    readRow = z;//正在读取的行
                    readCol = 0;//正在读取的列
                    var row = "<tr " + trMatchCollection[z].ToString().Trim() + "</tr>";
                    var tdReg = new Regex(pattern: @"(?<=(<[t|T][d|D|h|H]))[\s\S]*?(?=(</[t|T][d|D|h|H]>))");
                    var tdMatchCollection = tdReg.Matches(row);//获取行的列集合

                    IRow hssfrow = hssfSheet.CreateRow(readRow);//根据索引创建行

                    for (int c = 0; c < tdMatchCollection.Count; c++)
                    {
                        int CellColSpan = 1;//列的合并列数
                        int CellRowSpan = 1;//列的合并行数
                        MatchCollection mc = Regex.Matches(tdMatchCollection[c].ToString(), @"([^\s=]+)=(['""\s]?)([^'""]+)\2(?=\s|$|>)");//列数据集合
                        foreach (Match m in mc)
                        {
                            if (m.Groups[1].Value.ToLower() == "colspan")
                            {
                                CellColSpan = int.Parse(m.Groups[3].Value);//赋值列的合并行数
                            }
                            else if (m.Groups[1].Value.ToLower() == "rowspan")
                            {
                                CellRowSpan = int.Parse(m.Groups[3].Value);//赋值列的合并行数
                            }
                        }

                        //正在读取的行列
                        for (int y = 0; y < totalCol; y++)
                        {
                            if (excelTable[readRow][y].ToString().ToLower() == "false")
                            {
                                readCol = y;
                                break;
                            }
                        }


                        //判断行列是否有合并
                        if (CellColSpan * CellRowSpan > 1)
                        {
                            var cellRangeAddress = new CellRangeAddress(readRow, readRow + CellRowSpan - 1, readCol, readCol + CellColSpan - 1);
                            hssfSheet.AddMergedRegion(cellRangeAddress);//合并行列

                            var tdValue = RemoveHtml("<td " + tdMatchCollection[c].ToString().Trim() + "</td>");//过滤html格式获取td内容
                            hssfrow.CreateCell(readCol).SetCellValue(tdValue.Trim());//设置表格内容

                            for (int frow = readRow; frow <= CellRowSpan + readRow - 1; frow++)
                            {
                                for (int fcol = readCol; fcol <= CellColSpan + readCol - 1; fcol++)
                                {
                                    excelTable[frow][fcol] = true;
                                }
                            }
                            readCol = readCol + CellColSpan;
                        }
                        else
                        {

                            var cellRangeAddress = new CellRangeAddress(readRow, readRow, readCol, readCol);
                            hssfSheet.AddMergedRegion(cellRangeAddress);//合并行列
                            var tdValue = RemoveHtml("<td " + tdMatchCollection[c].ToString().Trim() + "</td>");//过滤html格式获取td内容
                            hssfrow.CreateCell(readCol).SetCellValue(tdValue.Trim());//设置表格内容

                            excelTable[readRow][readCol] = true;
                        }
                    }
                }


                MemoryStream ms = new MemoryStream();

                hssfworkbook.Write(ms);

                ms.Seek(0, SeekOrigin.Begin);

                HttpContext context = HttpContext.Current;
                string fileName = sheetName + "-" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xls";
                if (context.Request.Browser.Browser == "IE")
                    fileName = HttpUtility.UrlEncode(fileName);
                context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
                context.Response.BinaryWrite(ms.ToArray());


            }



        }


        public static ICellStyle HtmlTableCellStyle2ExcelCellStyle(HSSFWorkbook hssfworkbook, string html)
        {

            ICellStyle cellStyle = hssfworkbook.CreateCellStyle();
            MatchCollection mcAttr = Regex.Matches(html, @"([^\s=]+)=(['""\s]?)([^'""]+)\2(?=\s|$|>)");//列数据集合
            cellStyle.WrapText = true;
            foreach (Match m in mcAttr)
            {
                if (m.Groups[1].Value.ToLower() == "style")
                {

                    string[] mcStyle = m.Groups[3].Value.Split(';');

                    foreach (string ms in mcStyle)
                    {
                        string[] style = ms.Split(':');
                        if (style.Length == 2)
                        {
                            string key = style[0];
                            string value = style[1];
                            switch (key)
                            {
                                case "text-align":
                                    switch (value)
                                    {
                                        case "center":
                                            cellStyle.Alignment = HorizontalAlignment.Center;
                                            break;
                                        case "left":
                                            cellStyle.Alignment = HorizontalAlignment.Left;
                                            break;
                                        case "right":
                                            cellStyle.Alignment = HorizontalAlignment.Right;
                                            break;

                                    }
                                    break;
                                case "vertical-align":
                                    switch (value)
                                    {
                                        case "middle":
                                            cellStyle.VerticalAlignment = VerticalAlignment.Center;
                                            break;
                                        case "bottom":
                                            cellStyle.VerticalAlignment = VerticalAlignment.Bottom;
                                            break;
                                        case "top":
                                            cellStyle.VerticalAlignment = VerticalAlignment.Top;
                                            break;

                                    }
                                    break;

                            }
                        }
                    }
                }
            }
            return cellStyle;
        }


        /// <summary>  
        ///     去除HTML标记  
        /// </summary>  
        /// <param name="htmlstring"></param>  
        /// <returns>已经去除后的文字</returns>  
        public static string RemoveHtml(string htmlstring)
        {
            //删除脚本      
            htmlstring =
                Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>",
                              "", RegexOptions.IgnoreCase);
            //删除HTML      
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);


            htmlstring = htmlstring.Replace("<", "");
            htmlstring = htmlstring.Replace(">", "");
            htmlstring = htmlstring.Replace("\r\n", "");
            return htmlstring;
        }



        public static DataTable GetDataTable(string filepath, string tableName)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }


    }
}