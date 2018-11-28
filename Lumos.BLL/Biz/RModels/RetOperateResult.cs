using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class RetOperateResult
    {
        public RetOperateResult()
        {
            this.Fields = new List<Field>();
            this.Buttons = new List<Button>();
        }
        public ResultType Result { get; set; }
        public string Message { get; set; }
        public string Remarks { get; set; }
        public bool IsComplete { get; set; }
        public List<Field> Fields { get; set; }
        public List<Button> Buttons { get; set; }
        public class Field
        {
            public string Name
            {
                get; set;
            }

            public string Value
            {
                get; set;
            }
        }
        public class Button
        {

            public string Name
            {
                get; set;
            }
            public string Color
            {
                get; set;
            }

            public string Url
            {
                get; set;
            }

            public string Operate
            {
                get;set;
            }
        }
        public enum ResultType
        {
            Unknown = 0,
            Success = 1,
            Failure = 2,
            Exception = 3,
            Tips = 4
        }
    }
}
