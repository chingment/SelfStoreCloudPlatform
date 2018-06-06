using Lumos.Common;
using Lumos.DAL;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using System.IO;
using Newtonsoft.Json.Converters;
using Lumos.Mvc;

namespace WebBack
{

    /// <summary>
    /// BaseController用来扩展Controller,凡是在都该继承BaseController
    /// </summary>
    [OwnException]
    [OwnAuthorize]
    [ValidateInput(false)]
    public abstract class OwnBaseController : BaseController
    {

        public override string CurrentUserId
        {
            get
            {
                return OwnRequest.GetCurrentUserId();
            }
        }
    }
}