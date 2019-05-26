using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPlanMatcher
{
    class PlannedSwath
    {
        public float StartLat { get; set; }
        public float StartLong { get; set; }
        public float EndLat { get; set; }
        public float EndLong { get; set; }

        public int PlannedAltitude { get; set; }
        // flight run number according to KML flight plan
        public string PlannedOrder { get; set; }


    }
}
