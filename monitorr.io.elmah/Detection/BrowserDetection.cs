namespace monitorr.io.elmah.Detection
{
    public static class BrowserDetection
    {
        private const string IE = "Internet Explorer";

        private const string Edge = "Microsoft Edge";

        private const string Chrome = "Chrome";

        private const string FireFox = "FireFox";

        private const string Safari = "Safari";

        private const string Opera = "Opera";

        public static string Detect(string userAgent)
        {
            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident/"))
            {
                return IE;
            }

            if (userAgent.Contains("Firefox"))
            {
                return FireFox;
            }
            if (userAgent.Contains("OPR/") || userAgent.Contains("Opera/"))
            {
                return Opera;
            }
            if (userAgent.Contains("Safari/") && userAgent.Contains("Version/"))
            {
                return Safari;
            }
            if (userAgent.Contains("Edge/"))
            {
                return Edge;
            }
            if (userAgent.Contains("Chrome") || userAgent.Contains("CriOS/"))
            {
                return Chrome;
            }
            if (userAgent.Contains("Safari"))
            {
                return Safari;
            }
            return null;
        }
    }
}
