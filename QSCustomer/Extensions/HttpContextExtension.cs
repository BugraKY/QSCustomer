using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QSCustomer.Utility;

namespace QSCustomer.Extensions
{
    public static class HttpContextExtension
    {
        private static HttpContext _httpContext;
        public static void SetCookie(string key, string value)
        {
            //_httpContext.HttpContext.Response.Cookies.Append(key, value);
            _httpContext.Session.SetString(key, value);
        }
        public static void GetCookie(string key)
        {
            //_httpContext.HttpContext.Request
            var _aaaaaaa = _httpContext.Session.GetInt32(key);
            ProjectVariables.HttpContextVariables.HttpContxt_KEY = _httpContext.Session.GetString(key);

        }
    }
}
