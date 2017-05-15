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

        public static void Monitor(this Exception exception, HttpContext context,
            IDictionary<string, string> additionalData = null)
        {
            var logId = ConfigReader.Instance().LogId;
            if (logId == null)
            {
                throw new InvalidOperationException("Cannot get log ID.");
            }

            var client = new MonitorrClient();
            client.Log(logId.Value, context, exception, additionalData);
        }
    }
}
