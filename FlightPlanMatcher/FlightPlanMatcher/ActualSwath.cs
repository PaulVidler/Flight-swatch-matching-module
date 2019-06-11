using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPlanMatcher
{
    class ActualSwath
    {

        public double StartLat { get; set; }
        public double StartLong { get; set; }
        public double EndLat { get; set; }
        public double EndLong { get; set; }

        public int Altitude { get; set; }
        // flight run number according to the order it was actually flown - Comes from Riegl module
        public string ActualOrder { get; set; }
    }
}
