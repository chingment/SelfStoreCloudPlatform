﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMerch
{
    public class SelectControlFieldModel
    {
        public SelectControlFieldModel()
        {

        }

        public SelectControlFieldModel(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public SelectControlFieldModel(string name, string value, int type)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
        }

        public SelectControlFieldModel(string name, string value, string pValue)
        {
            this.Name = name;
            this.Value = value;
            this.PValue = pValue;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public string PValue { get; set; }


        public int Type { get; set; }

        public bool Disabled { get; set; }

        public int Dept { get; set; }
    }
}
