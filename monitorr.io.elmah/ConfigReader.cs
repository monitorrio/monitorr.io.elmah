using System;
using System.Configuration;
using System.IO;
using monitorr.io.elmah.Xml;

namespace monitorr.io.elmah
{
    public class ConfigReader
    {
        public static ConfigReader Instance()
        {
            if (_instance == null)
            {
                lock (typeof(ConfigReader))
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigReader();
                        var logId = GetErrorLogSection();
                        _instance.LogId = logId;
                    }
                }
            }
            return _instance;
        }

        protected ConfigReader() { }
        private static volatile ConfigReader _instance;

        public Guid? LogId { get; private set; }

        private static Guid? GetErrorLogSection()
        {
            try
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                string pathToConfigFile = Path.Combine(baseDirectory, "web.config");

                var configMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = pathToConfigFile
                };
                var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                var elmahSection = config.GetSection("elmah/errorLog");
                var xml = elmahSection.SectionInformation.GetRawXml();
                var obj = XmlConvert.DeserializeObject<ErrorLogXml>(xml);
                if (string.IsNullOrEmpty(obj?.LogId))
                {
                    return null;
                }
                return new Guid(obj.LogId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
