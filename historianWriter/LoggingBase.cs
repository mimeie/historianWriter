using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using NLog.Targets.ElasticSearch;

namespace historianWriter
{
    public class LoggingBase : MetricBase
    {
        public static Logger loggerBase;

        //probeweise loggen
        private NLog.Config.LoggingConfiguration config;
        private ElasticSearchTarget logelastic;


        public LoggingBase()
        {
            // Rules for mapping loggers to targets
            //config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logelastic);

            //// Apply config
            //NLog.LogManager.Configuration = config;
            //logger = NLog.LogManager.GetCurrentClassLogger();



            config = new NLog.Config.LoggingConfiguration();
            logelastic = new ElasticSearchTarget
            {
                Name = "elastic",
                Uri = "http://jhistorian.prod.j1:9200/",  //Uri = "http://192.168.2.41:32120", 
                Index = "historianWriter-${level}-${date:format=yyyy-MM-dd}",
                //Layout = "${logger} | ${threadid} | ${message}",
                Layout = "${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}|url: ${aspnet-request-url}|action: ${aspnet-mvc-action}",
                IncludeAllProperties = true,
            };
            // Rules for mapping loggers to targets
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logelastic);

            // Apply config
            NLog.LogManager.Configuration = config;
            loggerBase = NLog.LogManager.GetCurrentClassLogger();


            loggerBase.Info($"LoggingBase Klasse initialisiert");
        }
        
        
    }
}
