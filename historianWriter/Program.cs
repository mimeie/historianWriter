using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NLog;
using NLog.Targets.ElasticSearch;
using JusiBase;

namespace historianWriter
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();        

        public static void Main(string[] args)
        {


            ElasticSearchTarget elastictarget = new ElasticSearchTarget
            {
                Name = "elastic",
                Uri = "http://jhistorian.prod.j1:9200/",  //Uri = "http://192.168.2.41:32120", 
                Index = "historianWriter-Test-${level}-${date:format=yyyy-MM-dd}",
                //Index = "historianWriter-${level}-${date:format=yyyy-MM-dd}",
                //Layout = "${logger} | ${threadid} | ${message}",
                //Layout = "${longdate}|${event-properties:item=EventId_Id}|${threadid}|${uppercase:${level}}|${logger}|${hostname}|${message} ${exception:format=tostring}",
                Layout = "${message}",
            IncludeAllProperties = true,
            };
            
            JusiBase.LoggingBase logging = new LoggingBase(elastictarget, NLog.LogLevel.Debug, NLog.LogLevel.Fatal);

            logger                
                .Info($"Main Startup run");

            CreateHostBuilder(args).Build().Run();

            logger                
                .Info($"Main Startup beendet");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
