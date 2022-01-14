using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Extensions
{
    public class CookieExtensions
    {
        public static IHttpContextAccessor _hc;

        #region Filters

        public static class Filters
        {
            public static void SetFilters(bool open, bool close, bool problematic)
            {
                _hc.HttpContext.Response.Cookies.Append("kj6ght", open.ToString());
                _hc.HttpContext.Response.Cookies.Append("h4k9xp", close.ToString());
                //var cookie=_hc.HttpContext.Request.Cookies["_kj6ght"];

                CookieOptions option = new CookieOptions();

                //Response.Cookies.Append(key, value, option);
            }
            public static class GetFilters
            {
                static bool open = Convert.ToBoolean(_hc.HttpContext.Request.Cookies["_kj6ght"]);
                static bool close = Convert.ToBoolean(_hc.HttpContext.Request.Cookies["_h4k9xp"]);

                public static bool Open()
                {
                    return open;
                }
                public static bool Close()
                {
                    return close;
                }
            }
        }
        #endregion Filters

    }
}
