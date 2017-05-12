using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Elmah;
using monitorr.io.elmah.Detection;

namespace monitorr.io.elmah
{
    public class ErrorModelCreator
    {
        public static ErrorModel Create(Error error, string logId)
        {
            return new ErrorModel
            {
                Guid = Guid.NewGuid().ToString(),
                Detail = error.Detail,
                LogId = logId,
                Host = Host(error.ServerVariables),
                Type = error.Type,
                Source = error.Source,
                Message = error.Message,
                User = error.User,
                Time = error.Time,
                StatusCode = error.StatusCode,
                ServerVariables = MonitorrHelpers.ToDictionary(error.ServerVariables),
                QueryString = MonitorrHelpers.ToDictionary(error.QueryString),
                Form = MonitorrHelpers.ToDictionary(error.Form),
                Cookies = MonitorrHelpers.ToDictionary(error.Cookies),
                Browser = Browser(error.ServerVariables),
                Severity = GetSeverity(error.StatusCode),
                Url = Url(error.ServerVariables),
                IsCustom = false,
                Method =  Method(error.ServerVariables)
            };
        }

        public static ErrorModel Create(Guid logId, HttpContext context, Exception exception = null,
            IDictionary<string, string> additionalData = null)
        {
            return new ErrorModel
            {
                Guid = Guid.NewGuid().ToString(),
                Message = Message(exception),
                Time = DateTime.UtcNow,
                Detail = Detail(exception),
                Source = exception?.Source,
                Type = exception?.GetType().Name,
                Cookies = Cookies(context),
                Form = Form(context),
                Host = Host(context.Request),
                ServerVariables = ServerVariables(context),
                StatusCode = context.Response?.StatusCode,
                QueryString = QueryString(context),
                Method = context.Request?.HttpMethod,
                LogId = logId.ToString(),
                Browser = Browser(context.Request?.Headers),
                User = User(context),
                Severity = GetSeverity(context.Response?.StatusCode),
                Url = Url(context.Request?.ServerVariables),
                IsCustom = true,
                CustomData = additionalData
            };
        }


        private static string Method(NameValueCollection serverVariables)
        {
            var verb = serverVariables.Get("REQUEST_METHOD");
            return verb ?? "";
        }

        private static string Host(NameValueCollection serverVariables)
        {
            var host = serverVariables.Get("HTTP_HOST");
            return host ?? "";
        }

        private static string Host(HttpRequest request)
        {
            return request?.Url.GetLeftPart(UriPartial.Authority) ?? "";
        }

        private static string Url(NameValueCollection serverVariables)
        {
            if (serverVariables == null)
            {
                return null;
            }

            return serverVariables["URL"];
        }

        private static Severity GetSeverity(int? responseStatusCode)
        {
            if (responseStatusCode.HasValue && responseStatusCode == 500)
            {
                return Severity.Crytical;
            }

            return Severity.Warning;
        }

        private static string Browser(NameValueCollection headers)
        {
            var ua = headers["HTTP_USER_AGENT"] ?? headers["User-Agent"] ;

            if (ua != null)
            {
                return BrowserDetection.Detect(ua);
            }

            return null;
        }

        private static string User(HttpContext context)
        {
            return context.User?.Identity?.Name;
        }
        
        private static string Message(Exception exception)
        {
            if (exception == null)
            {
                return "Status code is unsuccessful";
            }
            return exception.Message;
        }

        private static string Detail(Exception exception)
        {
            return exception?.ToString();
        }

        private static Dictionary<string, string> Form(HttpContext context)
        {
            try
            {
                return MonitorrHelpers.ToDictionary(context.Request?.Form);
            }
            catch (InvalidOperationException)
            {
            }

            return null;
        }

        private static Dictionary<string, string> QueryString(HttpContext context)
        {
            return MonitorrHelpers.ToDictionary(context.Request?.QueryString);
        }

        private static Dictionary<string, string> ServerVariables(HttpContext context)
        {
            return MonitorrHelpers.ToDictionary(context.Request?.Headers);
        }

        private static Dictionary<string, string> Cookies(HttpContext context)
        {
            return MonitorrHelpers.ToDictionary(context.Request?.Cookies);
        }
    }
}