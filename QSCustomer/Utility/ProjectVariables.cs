using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Utility
{
    public static class ProjectVariables
    {
        public static double CountProgress { get; set; }
        public static double LengthProgress { get; set; }
        public static class HttpContextVariables
        {
            public static string HttpContxt_KEY { get; set; } public static string HttpContxt_VAL { get; set; }
        }
    }
}
