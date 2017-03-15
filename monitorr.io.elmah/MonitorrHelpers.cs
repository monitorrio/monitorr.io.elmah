using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace monitorr.io.elmah
{
    public static class MonitorrHelpers
    {
        public static Dictionary<string, string> ToDictionary(NameValueCollection col)
        {
            if (col == null)
            {
                return null;
            }

            var dict = new Dictionary<string, string>();
            foreach (var k in col.AllKeys)
            {
                dict.Add(k, col[k]);
            }
            return dict;
        }

        public static Dictionary<string, string> ToDictionary(HttpCookieCollection requestCookies)
        {
            if (requestCookies == null)
            {
                return null;
            }

            var dict = new Dictionary<string, string>();
            foreach (var k in requestCookies.AllKeys)
            {
                if (requestCookies[k] == null)
                {
                    continue;
                }
                dict.Add(k, requestCookies[k].Value);
            }
            return dict;
        }
    }
}
