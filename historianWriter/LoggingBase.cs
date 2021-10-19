using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using NLog.Targets.ElasticSearch;

namespace historianWriter
{
    public class LoggingBase
    {
        public static Logger logger;

        //probeweise loggen
        private NLog.Config.LoggingConfiguration config = new NLog.Config.LoggingConfiguration();
        private ElasticSearchTarget logelastic = new ElasticSearchTarget
        {
            Name = "elastic",
            Uri = "http://jhistorian.mei.local:9200/",  //Uri = "http://192.168.2.41:32120", 
            Index = "historianwriter",  //"app-${level}-${date:format=yyyy.MM.dd}"
            Layout = "${logger} | ${threadid} | ${message}",
            IncludeAllProperties = true,
        };


        public LoggingBase()
        {
            // Rules for mapping loggers to targets
            //config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logelastic);

            //// Apply config
            //NLog.LogManager.Configuration = config;
            //logger = NLog.LogManager.GetCurrentClassLogger();



            NLog.Config.LoggingConfiguration config = new NLog.Config.LoggingConfiguration();
            ElasticSearchTarget logelastic = new ElasticSearchTarget
            {
                Name = "elastic",
                Uri = "http://jhistorian.prod.j1:9200/",  //Uri = "http://192.168.2.41:32120", 
                Index = "historianwriter",  //"app-${level}-${date:format=yyyy.MM.dd}"
                Layout = "${logger} | ${threadid} | ${message}",
                IncludeAllProperties = true,
            };
            // Rules for mapping loggers to targets
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logelastic);

            // Apply config
            NLog.LogManager.Configuration = config;
            Logger logger = NLog.LogManager.GetCurrentClassLogger();
         

            logger.Info($"LoggingBase Klasse initialisiert");
        }
        
        
    }
}
