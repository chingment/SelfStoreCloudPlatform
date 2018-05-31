using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Home
{
    public class TaskByHandleSearchCondition:BaseSearchCondition
    {
        public int AuditStatus { get; set; }
    }
}