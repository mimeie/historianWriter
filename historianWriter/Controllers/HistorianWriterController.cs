using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using JusiBase;
using NLog;

namespace historianWriter
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistorianWriterController : ControllerBase
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MetricBase metric = new MetricBase();

        // api/lichtsteuerung/lichtankleide
        // /api/lichtsteuerung/allgemein?source=JemandZuhause
        [HttpGet("{id}", Name = "Get")]
        public ResponseTrigger Get(string id, string source)
        {

            try
            {
                metric.stopwatch.Start();


               
                    logger               
               .WithProperty("ObjektId", id)
               .Debug($"getter Aufruf mit id {id}");

                HistorianLogic.Instance.RaiseDataChange(id);

                  

                logger
               .WithProperty("Prozessdauer", metric.stopwatch.ElapsedMilliseconds)
               .WithProperty("ObjektId", id)
               .Debug($"Response verarbeitet id {id}");
                
                

                return new ResponseTrigger
                {
                    ReturnCode = 0,
                    //ReturnState = Hi.Instance.LichtsteuerungAnkleidezimmer.StateMachine.CurrentState.ToString()
                };
            }
            catch (Exception ex)
            {
                logger
               .Error($"Fehler bei LichtsteuerungAufruf", ex);


                return new ResponseTrigger
                {
                    ReturnCode = 0,
                    ReturnState = "Fehler"
                };

            }


        }
    }
}
