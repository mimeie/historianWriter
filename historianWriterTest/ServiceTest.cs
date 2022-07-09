using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using historianWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace historianWriterTest
{
    [TestClass]
    public class ServiceTest
    {
        [TestMethod]
        public void TestLoggerByEvent()
        {
            HistorianLogic.Instance.Start();
            HistorianLogic.Instance.RaiseDataChange("zigbee.0.00158d00045c0c7c.temperature");

        }
    }
}
