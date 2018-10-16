using log4net;
using Lumos.Common;
using Lumos.DAL;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Lumos.Entity;
using Lumos;

namespace WebTermApi
{

    public class OwnBaseApiController : BaseApiController
    {
        private APIResult _result = new APIResult();

        public APIResult Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public OwnBaseApiController()
        {
            LogUtil.SetTrackId();
        }

        public APIResponse ResponseResult(APIResult result)
        {
            return new APIResponse(result);
        }

        public APIResponse ResponseResult(ResultType resultType, ResultCode resultCode, string message = null, object data = null)
        {
            _result.Result = resultType;
            _result.Code = resultCode;
            _result.Message = message;
            _result.Data = data;

            return new APIResponse(_result);
        }

    }
}