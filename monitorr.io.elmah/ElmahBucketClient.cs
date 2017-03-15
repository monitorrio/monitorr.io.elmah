using System;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace monitorr.io.elmah
{
    public class MonitorrClient:IMonitorrClient
    {
        public string Version { get; set; } = "1";

        private WebClient _webClient;
        private Uri _baseUri;
        private Uri _postUrl;
        private JsonSerializerSettings _serializationSettings;

        public MonitorrClient()
        {
            Initialize();
        }

        private void Initialize()
        {

#if RELEASE
            _baseUri = new Uri("https://log.monitorr.io");
#elif STAGING
            _baseUri = new Uri("https://staging-log.monitorr.io");
#else
            _baseUri = new Uri("http://localhost:1900");
#endif
            _postUrl = new Uri(_baseUri, $"/v{Version}/errors");
            _webClient = new WebClient
            {
                Encoding = Encoding.UTF8
            };

            _webClient.Headers["Content-Type"] = "application/json";

            _serializationSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
           
        }

        internal void Log(ErrorModel errorModel)
        {
            try
            {
                var json = JsonConvert.SerializeObject(errorModel, _serializationSettings);
                _webClient.UploadString(_postUrl, "POST", json);
            }
            catch (Exception e)
            {
            }
        }

        public void Log(Guid logId, HttpContext context, Exception exception = null)
        {
            try
            {
                var errorModel = ErrorModelCreator.Create(logId, context, exception);
                var json = JsonConvert.SerializeObject(errorModel, _serializationSettings);

                _webClient.UploadString(_postUrl, "POST", json);
            }
            catch (Exception e)
            {
            }
        }
    }

    public interface IMonitorrClient
    {

        string Version { get; set; }

        void Log(Guid logId, HttpContext context, Exception exception = null);
    }
}
