using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QSCustomer.Utility;

namespace QSCustomer.Extensions
{
    public static class ProjectExtensions
    {
        public static IHttpContextAccessor httpContextAccessor;
        
        public static class FaultValues
        {
            public static string ProjectCode { get; set; }
            public static int Count { get; set; }
        }
        public static string GetUrl()
        {

            return "localhost:5001/";
        }
        /*
        public static void OnGet(string? key, string? value)
        {
            ProjectVariables.HttpContextVariables.HttpContxt_VAL = value;
        }*/
    }
}

