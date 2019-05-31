using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPlanMatcher
{
    class PlannedSwath
    {
        public double StartLat { get; set; }
        public double StartLong { get; set; }
        public double EndLat { get; set; }
        public double EndLong { get; set; }

        public int PlannedAltitude { get; set; }
        // flight run number according to KML flight plan
        public string PlannedOrder { get; set; }


    }
}
