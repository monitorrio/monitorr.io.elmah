using System;
using System.Collections.Generic;
using System.Web;

namespace monitorr.io.elmah.Extensions
{
    public static class ExceptionExtensions
    {
        public static void Monitor(this Exception exception, Guid logId, HttpContext context, 
            IDictionary<string, string> additionalData = null)
        {
            var client = new MonitorrClient();
            client.Log(logId, context, exception, additionalData);
        }
    }
}
