using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack
{
    public static class OwnResponse
    {
        public static void SetSession(string token)
        {
            HttpCookie cookie_session = HttpContext.Current.Request.Cookies[OwnRequest.SESSION_NAME];
            if (cookie_session != null)
            {
                cookie_session.Value = token;
                HttpContext.Current.Response.AppendCookie(cookie_session);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(OwnRequest.SESSION_NAME, token));
            }
        }
    }
}