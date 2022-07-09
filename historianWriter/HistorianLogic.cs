using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JusiBase;
using NLog;
//Update-Package


namespace historianWriter
{
    public sealed class HistorianLogic
    {
        private static volatile HistorianLogic _instance;
        private static object _syncRoot = new object();

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MetricBase metric = new MetricBase();


        public event EventHandler<string> DataChange;
        public Objekt HistorianObjekt;
        public IOBrokerClusterConnector clusterConn;


        public static HistorianLogic Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new HistorianLogic();
                        }
                    }
                }

                return _instance;
            }
        }

        public void Start()
        {
            metric.stopwatch.Restart();

            logger               
               .Info($"historianWriter Logic wird initalisiert");



            logger
               .WithProperty("Prozessdauer", metric.stopwatch.ElapsedMilliseconds)
               .Info($"historianWriter Logic gestartet");
        }

        public void Stop()
        {
        }

        public void RaiseDataChange(string ObjektId)
        {
            try
            {
                metric.stopwatch.Restart();
            DataChange?.Invoke(this, ObjektId);
                                  IOBrokerClusterConnector clusterConn = new IOBrokerClusterConnector();

                //mehr über den Datenpunkt erfahren
                IOBrokerJSONGet jsonResult = clusterConn.GetIOBrokerValue(ObjektId);
            if (jsonResult == null)
            {
                logger
               .WithProperty("ObjektId", ObjektId)
               .WithProperty("Prozessdauer", metric.stopwatch.ElapsedMilliseconds)
               .Warn($"Keine Daten von Iobroker erhalten für Objekt {ObjektId}");
                return;
            }

                //das Hauptobjekt finden (kann mehrere Ebenen nach oben sein)
                string parent = ObjektId;
                int idx = parent.LastIndexOf('.');
                parent = parent.Substring(0,idx);
                IOBrokerJSONGet jsonResultParent = clusterConn.GetIOBrokerValue(parent);
                if (jsonResultParent == null)
                {
                    logger
                   .WithProperty("ObjektId", parent)
                   .WithProperty("Prozessdauer", metric.stopwatch.ElapsedMilliseconds)
                   .Warn($"Keine Daten von Iobroker erhalten für Objekt {parent}");
                    return;
                }


                //.value von der typisierung entfernen
                string sourceString = jsonResult.common.role;
                string removeString = "value.";
                int index = sourceString.IndexOf(removeString);
                string cleanString = (index < 0)
                    ? sourceString
                    : sourceString.Remove(index, removeString.Length);


                //finaler wert in DB schreiben
                logger
               .WithProperty("ObjektId", ObjektId)               
               .WithProperty("Prozessdauer", metric.stopwatch.ElapsedMilliseconds)
               .WithProperty("HistorianObjektName", jsonResultParent.common.name + ' ' + jsonResult.common.name)
               .WithProperty("HistorianParentObjektId", parent)
               .WithProperty("HistorianParentName", jsonResultParent.common.name)
               .WithProperty("HistorianValue", jsonResult.val)
               .WithProperty("HistorianValueInt", jsonResult.valInt)
               .WithProperty("HistorianValueDouble", jsonResult.valDouble)
               .WithProperty("HistorianValueBool", jsonResult.valBool)
               .WithProperty("HistorianValueUnit", jsonResult.common.unit)
               .WithProperty("HistorianValueType", cleanString)
               //.WithProperty("HistorianValueString", jsonResult.valString)
               .WithProperty("HistorianTimestamp", jsonResult.LastChange)
               .WithProperty("HistorianLastChange", jsonResult.TimeStamp)
               .WithProperty("HistorianAdapter", jsonResult.from)
               .Info($"Datachange von id ausgeführt {ObjektId}");

            }
            catch (Exception ex)
            {
                logger
               .Error($"Fehler beim Datenupunkt loggen", ex);
                //throw;
            }


        }
    }
}
