using System;
using System.Web;

namespace monitorr.io.elmah.Extensions
{
    public static class ExceptionExtensions
    {
        public static void Monitor(this Exception exception, Guid logId, HttpContext context)
        {
            var client = new MonitorrClient();
            client.Log(logId, context, exception);
        }
    }
}
