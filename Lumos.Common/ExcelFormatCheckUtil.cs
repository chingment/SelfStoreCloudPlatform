using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Common
{

    public class CheckCell
    {
        public int Index { get; set; }

        public string Title { get; set; }

        public int MaxLength { get; set; }

        public int MinLength { get; set; }

        public ExcelCellValueType ValueType { get; set; }

    }

    public enum ExcelCellValueType
    {
        Unknow = 0,
        String = 1
    }

    public class ExcelFormatCheckUtil
    {
        private string _errorMessage;
        private List<string> _errorPoint;
        private List<CheckCell> _checkCell;
        private ISheet _sheet;
        public ISheet Sheet { get; set; }
        public ExcelFormatCheckUtil(ISheet sheet)
        {
            _sheet = sheet;
            _errorMessage = null;
            _errorPoint = new List<string>();
            _checkCell = new List<CheckCell>();
        }

        public void StartCheck()
        {
            IRow rowTitle = _sheet.GetRow(0);

            foreach (var cell in _checkCell)
            {
                var iCell = rowTitle.GetCell(cell.Index) == null ? "" : rowTitle.GetCell(cell.Index).ToString().Trim();
                if (iCell != cell.Title.Trim())
                {
                    _errorPoint.Add(string.Format("标题:{0},位置:{1},与模板不对应", cell.Title, (cell.Index + 1)));
                }
            }

            int rowCount = _sheet.LastRowNum + 1;
            int cellCount = rowTitle.LastCellNum;

            if (cellCount != _checkCell.Count)
            {
                _errorPoint.Add("导入文件的标题栏目数量是" + cellCount + "与模板标题栏目数量是" + _checkCell.Count + "，不一致");
            }

            if (_errorPoint.Count > 0)
            {
                _errorMessage = "模板不符合要求";
                return;
            }




            for (int i = 1; i < rowCount; i++)
            {
                IRow row = _sheet.GetRow(i);
                for (int j = 0; j < cellCount; j++)
                {
                    string value = row.GetCell(j) == null ? "" : row.GetCell(j).ToString();
                    var checkCell = _checkCell[j];
                    if (checkCell.ValueType == ExcelCellValueType.String)
                    {
                        if (checkCell.MinLength >= 1)
                        {
                            if (value.Length <= checkCell.MinLength || value.Length > checkCell.MaxLength)
                            {
                                _errorPoint.Add(string.Format("第{0}行的{1}的长度不符合要求,格式为{2}-{3}个字符", i, checkCell.Title, checkCell.MinLength, checkCell.MaxLength));
                            }
                        }
                    }
                }
            }

            if (_errorPoint.Count > 0)
            {
                _errorMessage = "数据格式不符合要求";
            }
        }

        public void AddCheckCellIsString(int index, string title, int minLen, int maxLen)
        {
            _checkCell.Add(new Common.CheckCell { Index = index, Title = title, MinLength = minLen, MaxLength = maxLen, ValueType = ExcelCellValueType.String });
        }


        public List<string> ErrorPoint
        {

            get
            {
                return _errorPoint;
            }
        }

        public string ErrorMessage
        {

            get
            {
                return _errorMessage;
            }
        }

    }
}
