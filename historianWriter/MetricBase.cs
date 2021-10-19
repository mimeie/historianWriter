using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Diagnostics;

namespace historianWriter
{
    public class MetricBase
    {
        public Stopwatch stopwatch;

        public MetricBase()
            {
            stopwatch = new Stopwatch();

            }

    }
}
