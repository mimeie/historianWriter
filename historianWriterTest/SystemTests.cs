using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using NLog;
using NLog.Targets.ElasticSearch;
using System.Threading.Tasks;

namespace historianWriterTest
{
    [TestClass]
    public class SystemTests
    {
        [TestMethod]
        public void TestLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logelastic = new ElasticSearchTarget
            {
                Name = "elastic",
                Uri = "http://jhistorian.prod.j1:9200/",  //Uri = "http://192.168.2.41:32120", 
                Index = "historianwriter-Test",  //"app-${level}-${date:format=yyyy.MM.dd}"
                Layout = "${logger} | ${threadid} | ${message}",
                IncludeAllProperties = true,
            };
            // Rules for mapping loggers to targets
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logelastic);

            // Apply config
            NLog.LogManager.Configuration = config;
            var logger = NLog.LogManager.GetCurrentClassLogger();


            logger.Info($"historianWriter unit test getestet");
        }

        [TestMethod]
        public void TestValueLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logelastic = new ElasticSearchTarget
            {
                Name = "elastic",
                Uri = "http://jhistorian.prod.j1:9200/",  //Uri = "http://192.168.2.41:32120", 
                Index = "historianwriter-Test",  //"app-${level}-${date:format=yyyy.MM.dd}"
                Layout = "${logger} | ${threadid} | ${message}",
                IncludeAllProperties = true,
            };
            // Rules for mapping loggers to targets
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logelastic);

            // Apply config
            NLog.LogManager.Configuration = config;
            var logger = NLog.LogManager.GetCurrentClassLogger();


            //var result = Parallel.For(1, 500, (i, state) =>
            for (int i = 0; i < 500; i++)
            {
                //logger.Info($"Beginning iteration {i}");             
                //logger.Info($"Completed iteration {i}");
                //logger.Info("fortestparallel {0}", i);

                //var eventInfo = new LogEventInfo(LogLevel.Info, logger.Name, "fortest parallel");
                //eventInfo.Properties["CustomValue"] = "My custom string";
                //eventInfo.Properties["CustomDateTimeValue"] = new DateTime(2020, 10, 30, 11, 26, 50);
                //eventInfo.Properties.Add("Logging Index", i);
                //logger.Log(eventInfo);

                Random rd = new Random();
                int rand_num = rd.Next(0, 500);


                int rand_num2 = rd.Next(500, 1000);


                logger.WithProperty("Wert", rand_num).WithProperty("Datenpunkt", "Datenpunkt1").Info($"es läuft ein value schreibe test");
                logger.WithProperty("Wert", rand_num2).WithProperty("Datenpunkt", "Datenpunkt2").Info($"es läuft ein value schreibe test");
                //logger.Info($"für 500ms schlafen");
                System.Threading.Thread.Sleep(500);
            }
            }
        }
    }

